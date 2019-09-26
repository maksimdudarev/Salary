using MD.Salary.WebApi.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;

namespace MD.Salary.WebApi.Tests.UnitTests.Moq
{
    public class UpdateEmployee : EmployeesControllerTests
    {
        [Fact]
        public void ReturnsBadRequestResult()
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
            var actResult = GetAsyncActionResult(() => _controller.UpdateEmployee(2000, createdItem));

            // Assert
            Assert.IsType<BadRequestResult>(actResult);
        }

        [Fact]
        public void ReturnsRedirectToActionResult()
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
            var redirectToActionResult = GetAsyncActionResult(() => _controller.UpdateEmployee(5000, createdItem));

            // Assert
            Assert.IsType<RedirectToActionResult>(redirectToActionResult);
        }
    }
}
