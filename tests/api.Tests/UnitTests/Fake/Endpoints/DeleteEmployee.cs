using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;

namespace MD.Salary.WebApi.Tests.UnitTests.Fake
{
    public class DeleteEmployee : EmployeesControllerTests
    {
        [Fact]
        public void NotExistingIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = GetAsyncActionResult(() => _controller.DeleteEmployee(2000));

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public void ExistingIdPassed_ReturnsRedirectToActionResult()
        {
            // Act
            var redirectToActionResult = GetAsyncActionResult(() => _controller.DeleteEmployee(1001));

            // Assert
            Assert.IsType<RedirectToActionResult>(redirectToActionResult);
        }

        [Fact]
        public void ExistingIdPassed_DeletesOneItem()
        {
            // Act
            GetAsyncActionResult(() => _controller.DeleteEmployee(1001));
            var items = GetAsyncActionResult(() => _service.ListBySearhstringAsync());

            // Assert
            Assert.Equal(2, items.Count());
        }
    }
}
