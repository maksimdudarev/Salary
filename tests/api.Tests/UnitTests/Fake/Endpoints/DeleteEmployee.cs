using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;
using static MD.Salary.WebApi.Tests.UnitTests.Constants;

namespace MD.Salary.WebApi.Tests.UnitTests.Fake
{
    public class DeleteEmployee : EmployeesControllerTests
    {
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
            var items = GetAsyncActionResult(() => _service.ListBySearhstringAsync());

            // Assert
            Assert.Equal(2, items.Count());
        }
    }
}
