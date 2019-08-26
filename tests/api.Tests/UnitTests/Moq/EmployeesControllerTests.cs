using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MD.Salary.WebApi.Controllers;
using MD.Salary.WebApi.Core.Interfaces;
using MD.Salary.WebApi.Core.Models;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;
using static MD.Salary.WebApi.Tests.UnitTests.Constants;

namespace MD.Salary.WebApi.Tests.UnitTests.Moq
{
    public class EmployeesControllerTestsWithMoq
    {
        readonly EmployeesController _controller;
        readonly Mock<IEmployeeRepository> _repository;

        public EmployeesControllerTestsWithMoq()
        {
            _repository = new Mock<IEmployeeRepository>();
            _repository.
                Setup(repo => repo.ListBySearhstringAsync("")).ReturnsAsync(GetTestEmployees());
            _repository.
                Setup(repo => repo.GetByIdAsync(ExistingId)).ReturnsAsync(ExistingItem);
            _controller = new EmployeesController(_repository.Object);
        }

        #region snippet_Index
        [Fact]
        public void Index_WhenCalled_ReturnsOkObjectResult()
        {
            // Act
            var okObjectResult = GetAsyncActionResult(() => _controller.Index(""));

            // Assert
            Assert.IsType<OkObjectResult>(okObjectResult);
        }

        [Fact]
        public void Index_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okObjectResult = GetAsyncActionResult(() => _controller.Index("")) as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<Employee>>(okObjectResult.Value);
            Assert.Equal(3, items.Count());
        }
        #endregion

        #region snippet_GetEmployee
        [Fact]
        public void GetEmployee_NotExistingIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = GetAsyncActionResult(() => _controller.GetEmployee(NotExistingId));

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public void GetEmployee_ExistingIdPassed_ReturnsOkObjectResult()
        {
            // Act
            var okObjectResult = GetAsyncActionResult(() => _controller.GetEmployee(ExistingId));

            // Assert
            Assert.IsType<OkObjectResult>(okObjectResult);
        }

        [Fact]
        public void GetEmployee_ExistingIdPassed_ReturnsRightItem()
        {
            // Act
            var okObjectResult = GetAsyncActionResult(() => _controller.GetEmployee(ExistingId)) as OkObjectResult;

            // Assert
            Assert.IsType<Employee>(okObjectResult.Value);
            Assert.Equal(ExistingId, (okObjectResult.Value as Employee).ID);
        }
        #endregion

        #region snippet_AddEmployee
        [Fact]
        public void AddEmployee_InvalidObjectPassed_ReturnsBadRequestObjectResult()
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
        public void AddEmployee_ValidObjectPassed_ReturnedResponseHasCreatedItem()
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
        #endregion

        #region snippet_DeleteEmployee
        [Fact]
        public void DeleteEmployee_NotExistingIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = GetAsyncActionResult(() => _controller.DeleteEmployee(NotExistingId));

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public void DeleteEmployee_ExistingIdPassed_ReturnsRedirectToActionResult()
        {
            // Act
            var redirectToActionResult = GetAsyncActionResult(() => _controller.DeleteEmployee(ExistingId));

            // Assert
            Assert.IsType<RedirectToActionResult>(redirectToActionResult);
        }
        [Fact]
        public void DeleteEmployee_ExistingIdPassed_DeletesOneItem()
        {
            // Act
            GetAsyncActionResult(() => _controller.DeleteEmployee(ExistingId));
            //var items = GetAsyncActionResult(() => _service.ListBySearhstringAsync());

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
