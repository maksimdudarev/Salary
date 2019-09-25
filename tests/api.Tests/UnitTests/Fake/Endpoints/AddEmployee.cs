using MD.Salary.WebApi.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;

namespace MD.Salary.WebApi.Tests.UnitTests.Fake
{
    public class AddEmployee : EmployeesControllerTests
    {
        [Fact]
        public void InvalidObjectPassed_ReturnsBadRequestObjectResult()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");
            Employee nameMissingItem = new Employee()
            {
                Group = "TestGroup",
                SalaryBase = 12.00M
            };

            // Act
            var badRequestObjectResult = GetAsyncActionResult(() => _controller.AddEmployee(nameMissingItem));

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestObjectResult);
        }

        [Fact]
        public void ValidObjectPassed_ReturnsCreatedAtActionResult()
        {
            // Arrange
            Employee createdItem = new Employee()
            {
                ID = 5000,
                Name = "CreatedItemName",
                Group = "TestGroup",
                SalaryBase = 12.00M
            };

            // Act
            var actResult = GetAsyncActionResult(() => _controller.AddEmployee(createdItem));

            // Assert
            Assert.IsType<CreatedAtActionResult>(actResult);
        }

        [Fact]
        public void ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            Employee createdItem = new Employee()
            {
                ID = 5000,
                Name = "CreatedItemName",
                Group = "TestGroup",
                SalaryBase = 12.00M
            };

            // Act
            var createdAtActionResult = GetAsyncActionResult(() => _controller.AddEmployee(createdItem)) as CreatedAtActionResult;
            var item = createdAtActionResult.Value as Employee;

            // Assert
            Assert.IsType<Employee>(item);
            Assert.Equal("CreatedItemName", item.Name);
        }
    }
}
