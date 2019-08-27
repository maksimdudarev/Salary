using Microsoft.AspNetCore.Mvc;
using Moq;
using MD.Salary.WebApi.Controllers;
using MD.Salary.WebApi.Core.Models;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;
using static MD.Salary.WebApi.Tests.UnitTests.Constants;

namespace MD.Salary.WebApi.Tests.UnitTests.Moq
{
    public class GetEmployee : EmployeesControllerTests
    {
        readonly EmployeesController _controller;

        public GetEmployee()
        {
            _repository.Setup(repo => repo.GetByIdAsync(ExistingId)).ReturnsAsync(ExistingItem);
            _controller = new EmployeesController(_repository.Object);
        }

        [Fact]
        public void NotExistingIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = GetAsyncActionResult(() => _controller.GetEmployee(NotExistingId));

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public void ExistingIdPassed_ReturnsOkObjectResult()
        {
            // Act
            var okObjectResult = GetAsyncActionResult(() => _controller.GetEmployee(ExistingId));

            // Assert
            Assert.IsType<OkObjectResult>(okObjectResult);
        }

        [Fact]
        public void ExistingIdPassed_ReturnsRightItem()
        {
            // Act
            var okObjectResult = GetAsyncActionResult(() => _controller.GetEmployee(ExistingId)) as OkObjectResult;

            // Assert
            Assert.IsType<Employee>(okObjectResult.Value);
            Assert.Equal(ExistingId, (okObjectResult.Value as Employee).ID);
        }
    }
}
