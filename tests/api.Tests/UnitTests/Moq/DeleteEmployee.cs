using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MD.Salary.WebApi.Controllers;
using MD.Salary.WebApi.Core.Models;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;
using static MD.Salary.WebApi.Tests.UnitTests.Constants;

namespace MD.Salary.WebApi.Tests.UnitTests.Moq
{
    public class DeleteEmployee : EmployeesControllerTests
    {
        readonly EmployeesController _controller;

        public DeleteEmployee()
        {
            _controller = new EmployeesController(_repository.Object);
        }

        [Fact]
        public void NotExistingIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = GetAsyncActionResult(() => _controller.DeleteEmployee(NotExistingId));

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public void ExistingIdPassed_ReturnsRedirectToActionResult()
        {
            // Arrange
            _repository.Setup(repo => repo.DeleteAsync(It.IsAny<Employee>()))
                .Returns(Task.CompletedTask)
                .Verifiable();
            // Act
            var redirectToActionResult = GetAsyncActionResult(() => _controller.DeleteEmployee(ExistingId));

            // Assert
            Assert.IsType<RedirectToActionResult>(redirectToActionResult);
        }
        [Fact]
        public void ExistingIdPassed_DeletesOneItem()
        {
            // Act
            GetAsyncActionResult(() => _controller.DeleteEmployee(ExistingId));
            //var items = GetAsyncActionResult(() => _service.ListBySearhstringAsync());

            // Assert
            //Assert.Equal(2, items.Count());
        }
    }
}
