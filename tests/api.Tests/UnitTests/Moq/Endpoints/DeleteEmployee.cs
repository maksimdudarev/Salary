using MD.Salary.WebApi.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;

namespace MD.Salary.WebApi.Tests.UnitTests.Moq
{
    public class DeleteEmployee : EmployeesControllerTests
    {
        [Fact]
        public void ReturnsNotFoundResult()
        {
            // Act
            var actResult = GetAsyncActionResult(() => _controller.DeleteEmployee(2000));

            // Assert
            Assert.IsType<NotFoundResult>(actResult);
        }

        [Fact]
        public void ReturnsRedirectToActionResult()
        {
            // Arrange
            Employee existingItem = new Employee()
            {
                ID = 1001,
                Name = "CreatedItemName",
            };
            _repository.Setup(repo => repo.GetEmployeeByIdAsync(1001)).ReturnsAsync(existingItem);

            // Act
            var actResult = GetAsyncActionResult(() => _controller.DeleteEmployee(1001));

            // Assert
            Assert.IsType<RedirectToActionResult>(actResult);
        }
    }
}
