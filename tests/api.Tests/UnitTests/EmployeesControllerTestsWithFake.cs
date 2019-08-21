using MD.Salary.WebApi.Controllers;
using MD.Salary.WebApi.Core.Interfaces;
using MD.Salary.WebApi.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MD.Salary.WebApi.Tests.UnitTests
{
    public class EmployeesControllerTestsWithFake
    {
        readonly EmployeesController _controller;
        readonly IEmployeeRepository _service;

        public EmployeesControllerTestsWithFake()
        {
            _service = new EmployeeRepositoryFake(ConstantsTests.GetTestEmployees());
            _controller = new EmployeesController(_service);
        }

        #region snippet_Index
        [Fact]
        public void Index_WhenCalled_ReturnsOkObjectResult()
        {
            // Act
            var okObjectResult = GetAsyncActionResult(() => _controller.Index(""));

            // Assert
            Assert.IsType<OkObjectResult>(okObjectResult);
        }

        [Fact]
        public void Index_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okObjectResult = GetAsyncActionResult(() => _controller.Index("")) as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<Employee>>(okObjectResult.Value);
            Assert.Equal(3, items.Count());
        }
        #endregion

        #region snippet_GetEmployee
        [Fact]
        public void GetEmployee_NotExistingIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = GetAsyncActionResult(() => _controller.GetEmployee(ConstantsTests.NotExistingId));

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public void GetEmployee_ExistingIdPassed_ReturnsOkObjectResult()
        {
            // Act
            var okObjectResult = GetAsyncActionResult(() => _controller.GetEmployee(ConstantsTests.ExistingId));

            // Assert
            Assert.IsType<OkObjectResult>(okObjectResult);
        }

        [Fact]
        public void GetEmployee_ExistingIdPassed_ReturnsRightItem()
        {
            // Act
            var okObjectResult = GetAsyncActionResult(() => _controller.GetEmployee(ConstantsTests.ExistingId)) as OkObjectResult;

            // Assert
            Assert.IsType<Employee>(okObjectResult.Value);
            Assert.Equal(ConstantsTests.ExistingId, (okObjectResult.Value as Employee).ID);
        }
        #endregion

        #region snippet_AddEmployee
        [Fact]
        public void AddEmployee_InvalidObjectPassed_ReturnsBadRequestObjectResult()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var badRequestObjectResult = GetAsyncActionResult(() => _controller.AddEmployee(ConstantsTests.NameMissingItem));

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestObjectResult);
        }

        [Fact]
        public void AddEmployee_ValidObjectPassed_ReturnsCreatedAtActionResult()
        {
            // Act
            var actResult = GetAsyncActionResult(() => _controller.AddEmployee(ConstantsTests.CreatedItem));

            // Assert
            Assert.IsType<CreatedAtActionResult>(actResult);
        }

        [Fact]
        public void AddEmployee_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Act
            var createdAtActionResult = GetAsyncActionResult(() => _controller.AddEmployee(ConstantsTests.CreatedItem)) as CreatedAtActionResult;
            var item = createdAtActionResult.Value as Employee;

            // Assert
            Assert.IsType<Employee>(item);
            Assert.Equal(ConstantsTests.CreatedItemName, item.Name);
        }
        #endregion

        #region snippet_UpdateEmployee
        [Fact]
        public void UpdateEmployee_InvalidObjectPassed_ReturnsBadRequestResult()
        {
            // Act
            var badRequestResult = GetAsyncActionResult(() => _controller.UpdateEmployee(ConstantsTests.NotExistingId, ConstantsTests.CreatedItem));

            // Assert
            Assert.IsType<BadRequestResult>(badRequestResult);
        }

        [Fact]
        public void UpdateEmployee_ValidObjectPassed_ReturnsRedirectToActionResult()
        {
            // Act
            var actResult = GetAsyncActionResult(() => _controller.UpdateEmployee(ConstantsTests.CreatedItemId, ConstantsTests.CreatedItem));

            // Assert
            Assert.IsType<RedirectToActionResult>(actResult);
        }

        [Fact]
        public void UpdateEmployee_ValidObjectPassed_ReturnedResponseHasUpdatedItem()
        {
            // Act
            GetAsyncActionResult(() => _controller.UpdateEmployee(ConstantsTests.ExistingId, ConstantsTests.ExistingItem));
            var item = GetAsyncActionResult(() => _service.GetByIdAsync(ConstantsTests.ExistingId));

            // Assert
            Assert.Equal(ConstantsTests.CreatedItemName, item.Name);
        }
        #endregion

        #region snippet_DeleteEmployee
        [Fact]
        public void DeleteEmployee_NotExistingIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = GetAsyncActionResult(() => _controller.DeleteEmployee(ConstantsTests.NotExistingId));

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public void DeleteEmployee_ExistingIdPassed_ReturnsRedirectToActionResult()
        {
            // Act
            var redirectToActionResult = GetAsyncActionResult(() => _controller.DeleteEmployee(ConstantsTests.ExistingId));

            // Assert
            Assert.IsType<RedirectToActionResult>(redirectToActionResult);
        }

        [Fact]
        public void DeleteEmployee_ExistingIdPassed_DeletesOneItem()
        {
            // Act
            GetAsyncActionResult(() => _controller.DeleteEmployee(ConstantsTests.ExistingId));
            var items = GetAsyncActionResult(() => _service.ListBySearhstringAsync());

            // Assert
            Assert.Equal(2, items.Count());
        }
        #endregion

        private T GetAsyncActionResult<T>(Func<Task<T>> func)
        {
            var task = func();
            task.Wait();
            return task.Result;
        }
    }
}
