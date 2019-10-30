using MD.Salary.WebApi.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;

namespace MD.Salary.WebApi.Tests.UnitTests.Moq.Authentication
{
    public class Logout : AuthenticationControllerTests
    {
        [Fact]
        public void ReturnsOkResult()
        {
            // Arrange
            Token existingItem = new Token()
            {
                ID = 1001,
                Value = "CreatedItemName",
            };
            _repository.Setup(repo => repo.GetTokenByValueAsync("CreatedItemName")).ReturnsAsync(existingItem);

            // Act
            var actResult = GetAsyncActionResult(() => _controller.Logout());

            // Assert
            Assert.IsType<OkResult>(actResult);
        }

    }
}
