using Microsoft.AspNetCore.Mvc;
using Moq;
using MD.Salary.WebApi.Core.Models;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;

namespace MD.Salary.WebApi.Tests.UnitTests.Moq
{
    public class GetEmployee : EmployeesControllerTests
    {
        public GetEmployee()
        {
            Employee existingItem = new Employee()
            {
                ID = 1001,
                Name = "CreatedItemName",
            };
            _repository.Setup(repo => repo.GetByIdAsync(1001)).ReturnsAsync(existingItem);
        }

        [Fact]
        public void ReturnsNotFoundResult()
        {
            // Act
            var actResult = GetAsyncActionResult(() => _controller.GetEmployee(2000));

            // Assert
            Assert.IsType<NotFoundResult>(actResult);
        }

        [Fact]
        public void ReturnsOkObjectResult()
        {
            // Act
            var actResult = GetAsyncActionResult(() => _controller.GetEmployee(1001));

            // Assert
            Assert.IsType<OkObjectResult>(actResult);
        }
    }
}
