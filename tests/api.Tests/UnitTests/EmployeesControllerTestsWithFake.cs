using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MD.Salary.WebApi.Controllers;
using MD.Salary.WebApi.Core.Interfaces;
using MD.Salary.WebApi.Core.Models;
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
        public async Task Index_WhenCalled_ReturnsOkObjectResult()
        {
            // Act
            var okObjectResult = await _controller.Index("");

            // Assert
            Assert.IsType<OkObjectResult>(okObjectResult);
        }

        [Fact]
        public async Task Index_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okObjectResult = await _controller.Index("") as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<Employee>>(okObjectResult.Value);
            Assert.Equal(3, items.Count());
        }
        #endregion

        #region snippet_GetEmployee
        [Fact]
        public async Task GetEmployee_NotExistingIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = await _controller.GetEmployee(ConstantsTests.NotExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async Task GetEmployee_ExistingIdPassed_ReturnsOkObjectResult()
        {
            // Act
            var okObjectResult = await _controller.GetEmployee(ConstantsTests.ExistingId);

            // Assert
            Assert.IsType<OkObjectResult>(okObjectResult);
        }

        [Fact]
        public async Task GetEmployee_ExistingIdPassed_ReturnsRightItem()
        {
            // Act
            var okObjectResult = await _controller.GetEmployee(ConstantsTests.ExistingId) as OkObjectResult;

            // Assert
            Assert.IsType<Employee>(okObjectResult.Value);
            Assert.Equal(ConstantsTests.ExistingId, (okObjectResult.Value as Employee).ID);
        }
        #endregion

        #region snippet_AddEmployee
        [Fact]
        public async Task AddEmployee_InvalidObjectPassed_ReturnsBadRequestObjectResult()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var badRequestObjectResult = await _controller.AddEmployee(ConstantsTests.nameMissingItem);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestObjectResult);
        }

        [Fact]
        public async Task AddEmployee_ValidObjectPassed_ReturnsCreatedAtActionResult()
        {
            // Act
            var createdResponse = await _controller.AddEmployee(ConstantsTests.createdItem);

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }

        [Fact]
        public async Task AddEmployee_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Act
            var createdResponse = await _controller.AddEmployee(ConstantsTests.createdItem) as CreatedAtActionResult;
            var item = createdResponse.Value as Employee;

            // Assert
            Assert.IsType<Employee>(item);
            Assert.Equal(ConstantsTests.createdItemName, item.Name);
        }
        #endregion

        #region snippet_DeleteEmployee
        [Fact]
        public async Task DeleteEmployee_NotExistingIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = await _controller.DeleteEmployee(ConstantsTests.NotExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async Task DeleteEmployee_ExistingIdPassed_ReturnsRedirectToActionResult()
        {
            // Act
            var redirectToActionResult = await _controller.DeleteEmployee(ConstantsTests.ExistingId);

            // Assert
            Assert.IsType<RedirectToActionResult>(redirectToActionResult);
        }
        [Fact]
        public async Task DeleteEmployee_ExistingIdPassed_DeletesOneItem()
        {
            // Act
            await _controller.DeleteEmployee(ConstantsTests.ExistingId);
            var items = await _service.ListBySearhstringAsync();

            // Assert
            Assert.Equal(2, items.Count());
        }
        #endregion
    }
}
