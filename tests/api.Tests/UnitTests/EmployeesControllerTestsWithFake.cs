using MD.Salary.WebApi.Controllers;
using MD.Salary.WebApi.Core.Interfaces;
using MD.Salary.WebApi.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;
using static MD.Salary.WebApi.Tests.UnitTests.Constants;

namespace MD.Salary.WebApi.Tests.UnitTests
{
    public class EmployeesControllerTestsWithFake
    {
        readonly EmployeesController _controller;
        readonly IEmployeeRepository _service;

        public EmployeesControllerTestsWithFake()
        {
            _service = new EmployeeRepositoryFake(GetTestEmployees());
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
            var notFoundResult = GetAsyncActionResult(() => _controller.GetEmployee(NotExistingId));

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public void GetEmployee_ExistingIdPassed_ReturnsOkObjectResult()
        {
            // Act
            var okObjectResult = GetAsyncActionResult(() => _controller.GetEmployee(ExistingId));

            // Assert
            Assert.IsType<OkObjectResult>(okObjectResult);
        }

        [Fact]
        public void GetEmployee_ExistingIdPassed_ReturnsRightItem()
        {
            // Act
            var okObjectResult = GetAsyncActionResult(() => _controller.GetEmployee(ExistingId)) as OkObjectResult;

            // Assert
            Assert.IsType<Employee>(okObjectResult.Value);
            Assert.Equal(ExistingId, (okObjectResult.Value as Employee).ID);
        }
        #endregion

        #region snippet_AddEmployee
        [Fact]
        public void AddEmployee_InvalidObjectPassed_ReturnsBadRequestObjectResult()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var badRequestObjectResult = GetAsyncActionResult(() => _controller.AddEmployee(NameMissingItem));

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestObjectResult);
        }

        [Fact]
        public void AddEmployee_ValidObjectPassed_ReturnsCreatedAtActionResult()
        {
            // Act
            var actResult = GetAsyncActionResult(() => _controller.AddEmployee(CreatedItem));

            // Assert
            Assert.IsType<CreatedAtActionResult>(actResult);
        }

        [Fact]
        public void AddEmployee_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Act
            var createdAtActionResult = GetAsyncActionResult(() => _controller.AddEmployee(CreatedItem)) as CreatedAtActionResult;
            var item = createdAtActionResult.Value as Employee;

            // Assert
            Assert.IsType<Employee>(item);
            Assert.Equal(CreatedItemName, item.Name);
        }
        #endregion

        #region snippet_UpdateEmployee
        [Fact]
        public void UpdateEmployee_InvalidObjectPassed_ReturnsBadRequestResult()
        {
            // Act
            var badRequestResult = GetAsyncActionResult(() => _controller.UpdateEmployee(NotExistingId, CreatedItem));

            // Assert
            Assert.IsType<BadRequestResult>(badRequestResult);
        }

        [Fact]
        public void UpdateEmployee_ValidObjectPassed_ReturnsRedirectToActionResult()
        {
            // Act
            var actResult = GetAsyncActionResult(() => _controller.UpdateEmployee(CreatedItemId, CreatedItem));

            // Assert
            Assert.IsType<RedirectToActionResult>(actResult);
        }

        [Fact]
        public void UpdateEmployee_ValidObjectPassed_ReturnedResponseHasUpdatedItem()
        {
            // Act
            GetAsyncActionResult(() => _controller.UpdateEmployee(ExistingId, ExistingItem));
            var item = GetAsyncActionResult(() => _service.GetByIdAsync(ExistingId));

            // Assert
            Assert.Equal(CreatedItemName, item.Name);
        }
        #endregion

        #region snippet_DeleteEmployee
        [Fact]
        public void DeleteEmployee_NotExistingIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = GetAsyncActionResult(() => _controller.DeleteEmployee(NotExistingId));

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public void DeleteEmployee_ExistingIdPassed_ReturnsRedirectToActionResult()
        {
            // Act
            var redirectToActionResult = GetAsyncActionResult(() => _controller.DeleteEmployee(ExistingId));

            // Assert
            Assert.IsType<RedirectToActionResult>(redirectToActionResult);
        }

        [Fact]
        public void DeleteEmployee_ExistingIdPassed_DeletesOneItem()
        {
            // Act
            GetAsyncActionResult(() => _controller.DeleteEmployee(ExistingId));
            var items = GetAsyncActionResult(() => _service.ListBySearhstringAsync());

            // Assert
            Assert.Equal(2, items.Count());
        }
        #endregion
    }
}
