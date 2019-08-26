using MD.Salary.WebApi.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;
using static MD.Salary.WebApi.Tests.UnitTests.Constants;

namespace MD.Salary.WebApi.Tests.UnitTests.Fake
{
    public class AddEmployee : EmployeesControllerTests
    {
        [Fact]
        public void InvalidObjectPassed_ReturnsBadRequestObjectResult()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var badRequestObjectResult = GetAsyncActionResult(() => _controller.AddEmployee(NameMissingItem));

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestObjectResult);
        }

        [Fact]
        public void ValidObjectPassed_ReturnsCreatedAtActionResult()
        {
            // Act
            var actResult = GetAsyncActionResult(() => _controller.AddEmployee(CreatedItem));

            // Assert
            Assert.IsType<CreatedAtActionResult>(actResult);
        }

        [Fact]
        public void ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Act
            var createdAtActionResult = GetAsyncActionResult(() => _controller.AddEmployee(CreatedItem)) as CreatedAtActionResult;
            var item = createdAtActionResult.Value as Employee;

            // Assert
            Assert.IsType<Employee>(item);
            Assert.Equal(CreatedItemName, item.Name);
        }
    }
}
