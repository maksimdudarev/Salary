using MD.Salary.WebApi.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;

namespace MD.Salary.WebApi.Tests.UnitTests.Moq.Authentication
{
    public class Register : AuthenticationControllerTests
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
            var actResult = GetAsyncActionResult(() => _controller.Register(nameMissingItem));

            // Assert
            Assert.IsType<BadRequestObjectResult>(actResult);
        }

        [Fact]
        public void ReturnsConflictResult()
        {
            // Arrange
            User existingItem = new User()
            {
                ID = 1001,
                Name = "CreatedItemName",
            };
            _repository.Setup(repo => repo.GetUserByNameAsync("CreatedItemName")).ReturnsAsync(existingItem);

            // Act
            var actResult = GetAsyncActionResult(() => _controller.Register(existingItem));

            // Assert
            Assert.IsType<ConflictResult>(actResult);
        }

        [Fact]
        public void ReturnsCreatedAtActionResult()
        {
            // Arrange
            User createdItem = new User()
            {
                ID = 5000,
                Name = "CreatedItemName",
                Password = "TestGroup",
                Role = 12
            };

            // Act
            var actResult = GetAsyncActionResult(() => _controller.Register(createdItem));

            // Assert
            Assert.IsType<CreatedAtActionResult>(actResult);
        }
    }
}
