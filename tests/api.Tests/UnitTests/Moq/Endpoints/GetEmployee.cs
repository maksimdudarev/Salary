using Microsoft.AspNetCore.Mvc;
using Moq;
using MD.Salary.WebApi.Core.Models;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;
using static MD.Salary.WebApi.Tests.UnitTests.Constants;

namespace MD.Salary.WebApi.Tests.UnitTests.Moq
{
    public class GetEmployee : EmployeesControllerTests
    {
        public GetEmployee()
        {
            _repository.Setup(repo => repo.GetByIdAsync(ExistingId)).ReturnsAsync(ExistingItem);
        }

        [Fact]
        public void ReturnsNotFoundResult()
        {
            // Act
            var actResult = GetAsyncActionResult(() => _controller.GetEmployee(NotExistingId));

            // Assert
            Assert.IsType<NotFoundResult>(actResult);
        }

        [Fact]
        public void ReturnsOkObjectResult()
        {
            // Act
            var actResult = GetAsyncActionResult(() => _controller.GetEmployee(ExistingId));

            // Assert
            Assert.IsType<OkObjectResult>(actResult);
        }
    }
}
