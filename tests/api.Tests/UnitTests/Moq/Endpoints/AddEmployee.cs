using Microsoft.AspNetCore.Mvc;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;
using static MD.Salary.WebApi.Tests.UnitTests.Constants;

namespace MD.Salary.WebApi.Tests.UnitTests.Moq
{
    public class AddEmployee : EmployeesControllerTests
    {
        [Fact]
        public void ReturnsBadRequestObjectResult()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var actResult = GetAsyncActionResult(() => _controller.AddEmployee(NameMissingItem));

            // Assert
            Assert.IsType<BadRequestObjectResult>(actResult);
        }

        [Fact]
        public void ReturnsCreatedAtActionResult()
        {
            // Act
            var actResult = GetAsyncActionResult(() => _controller.AddEmployee(CreatedItem));

            // Assert
            Assert.IsType<CreatedAtActionResult>(actResult);
        }
    }
}
