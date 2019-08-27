using Microsoft.AspNetCore.Mvc;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;
using static MD.Salary.WebApi.Tests.UnitTests.Constants;

namespace MD.Salary.WebApi.Tests.UnitTests.Fake
{
    public class UpdateEmployee : EmployeesControllerTests
    {
        [Fact]
        public void InvalidObjectPassed_ReturnsBadRequestResult()
        {
            // Act
            var badRequestResult = GetAsyncActionResult(() => _controller.UpdateEmployee(NotExistingId, CreatedItem));

            // Assert
            Assert.IsType<BadRequestResult>(badRequestResult);
        }

        [Fact]
        public void ValidObjectPassed_ReturnsRedirectToActionResult()
        {
            // Act
            var actResult = GetAsyncActionResult(() => _controller.UpdateEmployee(CreatedItemId, CreatedItem));

            // Assert
            Assert.IsType<RedirectToActionResult>(actResult);
        }

        [Fact]
        public void ValidObjectPassed_ReturnedResponseHasUpdatedItem()
        {
            // Act
            GetAsyncActionResult(() => _controller.UpdateEmployee(ExistingId, ExistingItem));
            var item = GetAsyncActionResult(() => _service.GetByIdAsync(ExistingId));

            // Assert
            Assert.Equal(CreatedItemName, item.Name);
        }
    }
}
