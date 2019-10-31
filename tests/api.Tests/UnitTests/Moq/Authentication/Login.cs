using System.Collections.Generic;
using MD.Salary.WebApi.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sodium;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;

namespace MD.Salary.WebApi.Tests.UnitTests.Moq.Authentication
{
    public class Login : AuthenticationControllerTests
    {
        [Fact]
        public void ReturnsBadRequestObjectResult()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");
            User nameMissingItem = new User()
            {
                Password = "TestGroup",
                Role = 12
            };

            // Act
            var actResult = GetAsyncActionResult(() => _controller.Login(nameMissingItem));

            // Assert
            Assert.IsType<BadRequestObjectResult>(actResult);
        }

        [Fact]
        public void ReturnsNotFoundResult()
        {
            // Arrange
            User existingItem = new User()
            {
                ID = 1001,
                Name = "CreatedItemName",
                Password = "TestGroup",
            };
            var items = new List<User> { existingItem };
            _repository.Setup(repo => repo.UserListAsync()).ReturnsAsync(items);

            // Act
            var actResult = GetAsyncActionResult(() => _controller.Login(existingItem));

            // Assert
            Assert.IsType<NotFoundResult>(actResult);
        }

        [Fact]
        public void ReturnsOkObjectResult()
        {
            // Arrange
            User createdItem = new User()
            {
                ID = 1001,
                Name = "CreatedItemName",
                Password = "TestGroup",
            };
            User ItemWithHashedPassword = new User()
            {
                Password = PasswordHash.ScryptHashString(createdItem.Password, PasswordHash.Strength.Medium),
            };
            var items = new List<User> { ItemWithHashedPassword };
            _repository.Setup(repo => repo.UserListAsync()).ReturnsAsync(items);

            // Act
            var actResult = GetAsyncActionResult(() => _controller.Login(createdItem));

            // Assert
            Assert.IsType<OkObjectResult>(actResult);
        }
    }
}
