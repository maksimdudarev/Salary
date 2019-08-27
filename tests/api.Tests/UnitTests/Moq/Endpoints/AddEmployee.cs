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
    public class AddEmployee : EmployeesControllerTests
    {
        readonly EmployeesController _controller;

        public AddEmployee()
        {
            _controller = new EmployeesController(_repository.Object);
        }

        [Fact]
        public void InvalidObjectPassed_ReturnsBadRequestObjectResult()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var actResult = GetAsyncActionResult(() => _controller.AddEmployee(NameMissingItem));

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actResult);
            Assert.IsType<SerializableError>(badRequestObjectResult.Value);
        }

        [Fact]
        public void ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            _repository.Setup(repo => repo.AddAsync(It.IsAny<Employee>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var actResult = GetAsyncActionResult(() => _controller.AddEmployee(CreatedItem));

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actResult);
            Assert.Null(createdAtActionResult.ControllerName);
            Assert.Equal("GetEmployee", createdAtActionResult.ActionName);
            _repository.Verify();
        }
    }
}
