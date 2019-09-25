using MD.Salary.WebApi.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;

namespace MD.Salary.WebApi.Tests.UnitTests.Fake
{
    public class UpdateEmployee : EmployeesControllerTests
    {
        [Fact]
        public void InvalidObjectPassed_ReturnsBadRequestResult()
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
            var badRequestResult = GetAsyncActionResult(() => _controller.UpdateEmployee(2000, createdItem));

            // Assert
            Assert.IsType<BadRequestResult>(badRequestResult);
        }

        [Fact]
        public void ValidObjectPassed_ReturnsRedirectToActionResult()
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
            var actResult = GetAsyncActionResult(() => _controller.UpdateEmployee(5000, createdItem));

            // Assert
            Assert.IsType<RedirectToActionResult>(actResult);
        }

        [Fact]
        public void ValidObjectPassed_ReturnedResponseHasUpdatedItem()
        {
            // Arrange
            Employee existingItem = new Employee()
            {
                ID = 1001,
                Name = "CreatedItemName",
            };

            // Act
            GetAsyncActionResult(() => _controller.UpdateEmployee(1001, existingItem));
            var item = GetAsyncActionResult(() => _service.GetByIdAsync(1001));

            // Assert
            Assert.Equal("CreatedItemName", item.Name);
        }
    }
}
