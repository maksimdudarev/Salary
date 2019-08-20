using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MD.Salary.WebApi.Controllers;
using MD.Salary.WebApi.Core.Interfaces;
using MD.Salary.WebApi.Core.Models;
using Xunit;

namespace MD.Salary.WebApi.Tests.UnitTests
{
    public class EmployeesControllerTestsWithMoq
    {
        readonly EmployeesController _controller;
        readonly Mock<IEmployeeRepository> _repository;

        public EmployeesControllerTestsWithMoq()
        {
            _repository = new Mock<IEmployeeRepository>();
            _repository.
                Setup(repo => repo.ListBySearhstringAsync("")).ReturnsAsync(ConstantsTests.GetTestEmployees());
            _repository.
                Setup(repo => repo.GetByIdAsync(ConstantsTests.ExistingId)).ReturnsAsync(ConstantsTests.ExistingItem);
            _controller = new EmployeesController(_repository.Object);
        }

        #region snippet_Index
        [Fact]
        public async Task Index_WhenCalled_ReturnsOkObjectResult()
        {
            // Act
            var okObjectResult = await _controller.Index("");

            // Assert
            Assert.IsType<OkObjectResult>(okObjectResult);
        }

        [Fact]
        public async Task Index_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okObjectResult = await _controller.Index("") as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<Employee>>(okObjectResult.Value);
            Assert.Equal(3, items.Count());
        }
        #endregion

        #region snippet_GetEmployee
        [Fact]
        public async Task GetEmployee_NotExistingIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = await _controller.GetEmployee(ConstantsTests.NotExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async Task GetEmployee_ExistingIdPassed_ReturnsOkObjectResult()
        {
            // Act
            var okObjectResult = await _controller.GetEmployee(ConstantsTests.ExistingId);

            // Assert
            Assert.IsType<OkObjectResult>(okObjectResult);
        }

        [Fact]
        public async Task GetEmployee_ExistingIdPassed_ReturnsRightItem()
        {
            // Act
            var okObjectResult = await _controller.GetEmployee(ConstantsTests.ExistingId) as OkObjectResult;

            // Assert
            Assert.IsType<Employee>(okObjectResult.Value);
            Assert.Equal(ConstantsTests.ExistingId, (okObjectResult.Value as Employee).ID);
        }
        #endregion

        #region snippet_AddEmployee
        [Fact]
        public async Task AddEmployee_InvalidObjectPassed_ReturnsBadRequestObjectResult()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var actResult = await _controller.AddEmployee(ConstantsTests.NameMissingItem);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actResult);
            Assert.IsType<SerializableError>(badRequestObjectResult.Value);
        }

        [Fact]
        public async Task AddEmployee_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            _repository.Setup(repo => repo.AddAsync(It.IsAny<Employee>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var actResult = await _controller.AddEmployee(ConstantsTests.CreatedItem);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actResult);
            Assert.Null(createdAtActionResult.ControllerName);
            Assert.Equal("GetEmployee", createdAtActionResult.ActionName);
            _repository.Verify();
        }
        #endregion

        #region snippet_DeleteEmployee
        [Fact]
        public async Task DeleteEmployee_NotExistingIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = await _controller.DeleteEmployee(ConstantsTests.NotExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async Task DeleteEmployee_ExistingIdPassed_ReturnsRedirectToActionResult()
        {
            // Act
            var redirectToActionResult = await _controller.DeleteEmployee(ConstantsTests.ExistingId);

            // Assert
            Assert.IsType<RedirectToActionResult>(redirectToActionResult);
        }
        [Fact]
        public async Task DeleteEmployee_ExistingIdPassed_DeletesOneItem()
        {
            // Act
            await _controller.DeleteEmployee(ConstantsTests.ExistingId);
            //var items = await _service.ListBySearhstringAsync();

            // Assert
            //Assert.Equal(2, items.Count());
        }
        #endregion
    }
    public interface IFoo
    {
        Bar Bar { get; set; }
        string Name { get; set; }
        int Value { get; set; }
        bool DoSomething(string value);
        bool DoSomething(int number, string value);
        string DoSomethingStringy(string value);
        bool TryParse(string value, out string outputValue);
        bool Submit(ref Bar bar);
        int GetCount();
        bool Add(int value);
    }
    public class Bar
    {
        public virtual Baz Baz { get; set; }
        public virtual bool Submit() { return false; }
    }
    public class Baz
    {
        public virtual string Name { get; set; }
    }
}
