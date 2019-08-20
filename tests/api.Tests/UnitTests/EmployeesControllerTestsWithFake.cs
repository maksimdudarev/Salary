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
            _service = new EmployeeRepositoryFake(HomeControllerTests.GetTestEmployees());
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
        // Arrange
        readonly long NotExistingId = 2000;
        readonly long ExistingId = 1001;

        [Fact]
        public async Task GetEmployee_NotExistingIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = await _controller.GetEmployee(NotExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async Task GetEmployee_ExistingIdPassed_ReturnsOkObjectResult()
        {
            // Act
            var okObjectResult = await _controller.GetEmployee(ExistingId);

            // Assert
            Assert.IsType<OkObjectResult>(okObjectResult);
        }

        [Fact]
        public async Task GetEmployee_ExistingIdPassed_ReturnsRightItem()
        {
            // Act
            var okObjectResult = await _controller.GetEmployee(ExistingId) as OkObjectResult;

            // Assert
            Assert.IsType<Employee>(okObjectResult.Value);
            Assert.Equal(ExistingId, (okObjectResult.Value as Employee).ID);
        }
        #endregion

        #region snippet_AddEmployee
        // Arrange
        readonly Employee nameMissingItem = new Employee()
        {
            Group = "Guinness",
            SalaryBase = 12.00M
        };
        readonly static string nameCreatedItem = "Guinness Original 6 Pack";
        readonly Employee createdItem = new Employee()
        {
            Name = nameCreatedItem,
            Group = "Guinness",
            SalaryBase = 12.00M
        };

        [Fact]
        public async Task AddEmployee_InvalidObjectPassed_ReturnsBadRequestObjectResult()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var badRequestObjectResult = await _controller.AddEmployee(nameMissingItem);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestObjectResult);
        }

        [Fact]
        public async Task AddEmployee_ValidObjectPassed_ReturnsCreatedAtActionResult()
        {
            // Act
            var createdResponse = await _controller.AddEmployee(createdItem);

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }

        [Fact]
        public async Task AddEmployee_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Act
            var createdResponse = await _controller.AddEmployee(createdItem) as CreatedAtActionResult;
            var item = createdResponse.Value as Employee;

            // Assert
            Assert.IsType<Employee>(item);
            Assert.Equal(nameCreatedItem, item.Name);
        }
        #endregion

        #region snippet_DeleteEmployee
        [Fact]
        public async Task DeleteEmployee_NotExistingIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = await _controller.DeleteEmployee(NotExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async Task DeleteEmployee_ExistingIdPassed_ReturnsRedirectToActionResult()
        {
            // Act
            var redirectToActionResult = await _controller.DeleteEmployee(ExistingId);

            // Assert
            Assert.IsType<RedirectToActionResult>(redirectToActionResult);
        }
        [Fact]
        public async Task DeleteEmployee_ExistingIdPassed_DeletesOneItem()
        {
            // Act
            await _controller.DeleteEmployee(ExistingId);
            var items = await _service.ListBySearhstringAsync();

            // Assert
            Assert.Equal(2, items.Count());
        }
        #endregion
    }
}
