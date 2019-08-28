using Microsoft.AspNetCore.Mvc;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;
using static MD.Salary.WebApi.Tests.UnitTests.Constants;

namespace MD.Salary.WebApi.Tests.UnitTests.Moq
{
    public class UpdateEmployee : EmployeesControllerTests
    {
        [Fact]
        public void ReturnsBadRequestResult()
        {
            // Act
            var actResult = GetAsyncActionResult(() => _controller.UpdateEmployee(NotExistingId, CreatedItem));

            // Assert
            Assert.IsType<BadRequestResult>(actResult);
        }

        [Fact]
        public void ReturnsRedirectToActionResult()
        {
            // Act
            var redirectToActionResult = GetAsyncActionResult(() => _controller.UpdateEmployee(CreatedItemId, CreatedItem));

            // Assert
            Assert.IsType<RedirectToActionResult>(redirectToActionResult);
        }
    }
}
