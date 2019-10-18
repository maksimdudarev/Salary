using MD.Salary.WebApi.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;

namespace MD.Salary.WebApi.Tests.UnitTests.Moq
{
    public class AddEmployee : EmployeesControllerTests
    {
        [Fact]
        public void ReturnsBadRequestObjectResult()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");
            Employee nameMissingItem = new Employee()
            {
                Group = "TestGroup",
                SalaryBase = 12.00M
            };

            // Act
            var actResult = GetAsyncActionResult(() => _controller.AddEmployee(nameMissingItem));

            // Assert
            Assert.IsType<BadRequestObjectResult>(actResult);
        }

        [Fact]
        public void ReturnsCreatedAtActionResult()
        {
            // Arrange
            Employee createdItem = new Employee()
            {
                UserId =  5000,
                Name = "CreatedItemName",
                Group = "TestGroup",
                SalaryBase = 12.00M
            };

            // Act
            var actResult = GetAsyncActionResult(() => _controller.AddEmployee(createdItem));

            // Assert
            Assert.IsType<CreatedAtActionResult>(actResult);
        }
    }
}
