using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MD.Salary.WebApi.Controllers;
using MD.Salary.WebApi.Core.Interfaces;
using MD.Salary.WebApi.Core.Models;
using MD.Salary.WebApi.Tests;
using Xunit;
using WebApiForTests.Contracts;
using WebApiForTests.Controllers;
using WebApiForTests.Model;
using WebApiForTests.Tests;

namespace TestingControllersSample.Tests.UnitTests
{
    public class ShoppingCartControllerTests
    {
        ShoppingCartController _controller;
        IShoppingCartService _service;
        EmployeesController _controllerE;
        IEmployeeRepository _serviceE;

        public ShoppingCartControllerTests()
        {
            _service = new ShoppingCartServiceFake();
            _controller = new ShoppingCartController(_service);
            _serviceE = new EmployeeRepositoryFake(HomeControllerTests.GetTestEmployees());
            _controllerE = new EmployeesController(_serviceE);
        }

        #region snippet_Index
        [Fact]
        public async Task Index_WhenCalled_ReturnsOkObjectResult()
        {
            // Act
            var okObjectResult = await _controllerE.Index("");

            // Assert
            Assert.IsType<OkObjectResult>(okObjectResult);
        }

        [Fact]
        public async Task Index_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okObjectResult = await _controllerE.Index("") as OkObjectResult;

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
            var notFoundResult = await _controllerE.GetEmployee(NotExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async Task GetEmployee_ExistingIdPassed_ReturnsOkObjectResult()
        {
            // Act
            var okObjectResult = await _controllerE.GetEmployee(ExistingId);

            // Assert
            Assert.IsType<OkObjectResult>(okObjectResult);
        }

        [Fact]
        public async Task GetEmployee_ExistingIdPassed_ReturnsRightItem()
        {
            // Act
            var okObjectResult = await _controllerE.GetEmployee(ExistingId) as OkObjectResult;

            // Assert
            Assert.IsType<Employee>(okObjectResult.Value);
            Assert.Equal(ExistingId, (okObjectResult.Value as Employee).ID);
        }
        #endregion

        #region snippet_AddEmployee
        // Arrange
        readonly Employee nameMissingItemE = new Employee()
        {
            Group = "Guinness",
            SalaryBase = 12.00M
        };
        readonly static string nameCreatedItem = "Guinness Original 6 Pack";
        readonly ShoppingItem createdItem = new ShoppingItem()
        {
            Name = nameCreatedItem,
            Manufacturer = "Guinness",
            Price = 12.00M
        };
        readonly Employee createdItemE = new Employee()
        {
            Name = nameCreatedItem,
            Group = "Guinness",
            SalaryBase = 12.00M
        };

        [Fact]
        public async Task AddEmployee_InvalidObjectPassed_ReturnsBadRequestObjectResult()
        {
            // Arrange
            _controllerE.ModelState.AddModelError("Name", "Required");

            // Act
            var badRequestObjectResultE = await _controllerE.AddEmployee(nameMissingItemE);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestObjectResultE);
        }

        [Fact]
        public async Task AddEmployee_ValidObjectPassed_ReturnsCreatedAtActionResult()
        {
            // Act
            var createdResponseE = await _controllerE.AddEmployee(createdItemE);

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponseE);
        }

        [Fact]
        public void AddEmployee_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Act
            var createdResponse = _controller.Post(createdItem) as CreatedAtActionResult;
            var item = createdResponse.Value as ShoppingItem;

            // Assert
            Assert.IsType<ShoppingItem>(item);
            Assert.Equal(nameCreatedItem, item.Name);
        }
        #endregion

        #region snippet_Remove
        // Arrange
        readonly Guid existingId = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

        [Fact]
        public void Remove_NotExistingIdPassed_ReturnsNotFoundResult()
        {
            // Arrange
            var notExistingId = Guid.NewGuid();

            // Act
            var notFoundResult = _controller.Remove(notExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public void Remove_ExistingIdPassed_ReturnsOkResult()
        {
            // Act
            var okResponse = _controller.Remove(existingId);

            // Assert
            Assert.IsType<OkResult>(okResponse);
        }
        [Fact]
        public void Remove_ExistingIdPassed_RemovesOneItem()
        {
            // Act
            var okResponse = _controller.Remove(existingId);

            // Assert
            Assert.Equal(2, _service.GetAllItems().Count());
        }
        #endregion
    }
}
