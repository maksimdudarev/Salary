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
            _serviceE = new EmployeeRepositoryFake(HomeControllerTests.GetTestEmployees2());
            _controllerE = new EmployeesController(_serviceE);
        }

        #region snippet_IndexStrong
        [Fact]
        public async Task IndexStrong_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = await _controllerE.IndexStrong("");

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public async Task IndexStrong_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = await _controllerE.IndexStrong("");
            var okObjectResult = okResult.Result as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<Employee>>(okObjectResult.Value);
            Assert.Equal(3, items.Count);
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
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public async Task GetEmployee_ExistingIdPassed_ReturnsOkResult()
        {
            // Act
            var okResult = await _controllerE.GetEmployee(ExistingId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public async Task GetEmployee_ExistingIdPassed_ReturnsRightItem()
        {
            // Act
            var okResult = await _controllerE.GetEmployee(ExistingId);
            var okObjectResult = okResult.Result as OkObjectResult;

            // Assert
            Assert.IsType<Employee>(okObjectResult.Value);
            Assert.Equal(ExistingId, (okObjectResult.Value as Employee).ID);
        }
        #endregion

        #region snippet_Post
        // Arrange
        readonly ShoppingItem nameMissingItem = new ShoppingItem()
        {
            Manufacturer = "Guinness",
            Price = 12.00M
        };
        readonly static string nameCreatedItem = "Guinness Original 6 Pack";
        readonly ShoppingItem createdItem = new ShoppingItem()
        {
            Name = nameCreatedItem,
            Manufacturer = "Guinness",
            Price = 12.00M
        };

        [Fact]
        public void Post_InvalidObjectPassed_ReturnsBadRequestResult()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var badRequestResult = _controller.Post(nameMissingItem);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
        }

        [Fact]
        public void Post_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Act
            var createdResponse = _controller.Post(createdItem);

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }

        [Fact]
        public void Post_ValidObjectPassed_ReturnedResponseHasCreatedItem()
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
