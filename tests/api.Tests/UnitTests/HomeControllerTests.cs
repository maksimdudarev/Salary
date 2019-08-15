using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TestingControllersSample.Controllers;
using TestingControllersSample.Core.Interfaces;
using TestingControllersSample.Core.Model;
using TestingControllersSample.ViewModels;
using MD.Salary.WebApi.Controllers;
using MD.Salary.WebApi.Core.Interfaces;
using MD.Salary.WebApi.Core.Models;
using Xunit;

namespace TestingControllersSample.Tests.UnitTests
{
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
    public class HomeControllerTests
    {
        #region snippet_Index_ReturnsAViewResult_WithAListOfBrainstormSessions
        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfBrainstormSessions()
        {
            // Arrange
            var mockRepo_old = new Mock<IBrainstormSessionRepository>();
            mockRepo_old.Setup(repo => repo.ListAsync())
                .ReturnsAsync(GetTestSessions());
            var controller_old = new HomeController(mockRepo_old.Object);

            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(repo => repo.ListAsync())
                .ReturnsAsync(GetTestEmployees());
            var controller = new EmployeesController(mockRepo.Object);

            // Act
            var result_old = await controller_old.Index();
            var result = await controller.Index("");

            // Assert
            var viewResult_old = Assert.IsType<ViewResult>(result_old);
            var model_old = Assert.IsAssignableFrom<IEnumerable<StormSessionViewModel>>(
                viewResult_old.ViewData.Model);
            Assert.Equal(2, model_old.Count());
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            //var model = Assert.IsAssignableFrom<List<Employee>>(okObjectResult.DeclaredType);
            var model = Assert.IsType<List<Employee>>(okObjectResult.Value);
            Assert.Equal(2, model.Count());
        }
        #endregion

        #region snippet_ModelState_ValidOrInvalid
        [Fact]
        public async Task IndexPost_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var mockRepo = new Mock<IBrainstormSessionRepository>();
            mockRepo.Setup(repo => repo.ListAsync())
                .ReturnsAsync(GetTestSessions());
            var controller = new HomeController(mockRepo.Object);
            controller.ModelState.AddModelError("SessionName", "Required");
            var newSession = new HomeController.NewSessionModel();

            // Act
            var result = await controller.Index(newSession);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task IndexPost_ReturnsARedirectAndAddsSession_WhenModelStateIsValid()
        {
            // Arrange
            var mockRepo = new Mock<IBrainstormSessionRepository>();
            mockRepo.Setup(repo => repo.AddAsync(It.IsAny<BrainstormSession>()))
                .Returns(Task.CompletedTask)
                .Verifiable();
            var controller = new HomeController(mockRepo.Object);
            var newSession = new HomeController.NewSessionModel()
            {
                SessionName = "Test Name"
            };

            // Act
            var result = await controller.Index(newSession);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mockRepo.Verify();
        }
        #endregion

        #region snippet_GetTestSessions
        private List<BrainstormSession> GetTestSessions()
        {
            var sessions = new List<BrainstormSession>();
            sessions.Add(new BrainstormSession()
            {
                DateCreated = new DateTime(2016, 7, 2),
                Id = 1,
                Name = "Test One"
            });
            sessions.Add(new BrainstormSession()
            {
                DateCreated = new DateTime(2016, 7, 1),
                Id = 2,
                Name = "Test Two"
            });
            return sessions;
        }
        #endregion

        #region snippet_GetTestEmployees
        private List<Employee> GetTestEmployees()
        {
            var employees = new List<Employee>();
            DateTimeOffset dateTimeOffset1 = new DateTime(2016, 7, 2);
            DateTimeOffset dateTimeOffset2 = new DateTime(2016, 7, 1);
            employees.Add(new Employee()
            {
                HireDate = dateTimeOffset1.ToUnixTimeSeconds(),
                ID = 1,
                Name = "Test One"
            });
            employees.Add(new Employee()
            {
                HireDate = dateTimeOffset2.ToUnixTimeSeconds(),
                ID = 2,
                Name = "Test Two"
            });
            return employees;
        }
        #endregion
    }
}
