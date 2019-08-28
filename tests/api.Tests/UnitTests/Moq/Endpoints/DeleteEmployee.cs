using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;
using static MD.Salary.WebApi.Tests.UnitTests.Constants;

namespace MD.Salary.WebApi.Tests.UnitTests.Moq
{
    public class DeleteEmployee : EmployeesControllerTests
    {
        [Fact]
        public void ReturnsNotFoundResult()
        {
            // Act
            var actResult = GetAsyncActionResult(() => _controller.DeleteEmployee(NotExistingId));

            // Assert
            Assert.IsType<NotFoundResult>(actResult);
        }

        [Fact]
        public void ReturnsRedirectToActionResult()
        {
            // Arrange
            _repository.Setup(repo => repo.GetByIdAsync(ExistingId)).ReturnsAsync(ExistingItem);

            // Act
            var actResult = GetAsyncActionResult(() => _controller.DeleteEmployee(ExistingId));

            // Assert
            Assert.IsType<RedirectToActionResult>(actResult);
        }
    }
}
