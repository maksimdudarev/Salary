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
    public class HomeControllerTests
    {
        HomeController _homeController;
        Mock<IBrainstormSessionRepository> _repositoryBrainstormSession;
        EmployeesController _employeesController;
        Mock<IEmployeeRepository> _employeeRepository;

        public HomeControllerTests()
        {
            _repositoryBrainstormSession = new Mock<IBrainstormSessionRepository>();
            _repositoryBrainstormSession.
                Setup(repo => repo.ListAsync()).ReturnsAsync(GetTestSessions());
            _homeController = new HomeController(_repositoryBrainstormSession.Object);
            _employeeRepository = new Mock<IEmployeeRepository>();
            _employeeRepository.
                Setup(repo => repo.ListBySearhstringAsync("")).ReturnsAsync(GetTestEmployees());
            _employeesController = new EmployeesController(_employeeRepository.Object);
        }

        #region snippet_Index_ReturnsAViewResult_WithAListOfBrainstormSessions
        [Fact]
        public async Task IndexStrong_ReturnsOkObjectResult()
        {
            // Act
            var okObjectResult = await _employeesController.IndexStrong("");

            // Assert
            Assert.IsType<OkObjectResult>(okObjectResult.Result);
        }
        [Fact]
        public async Task Index_ReturnsOkObjectResult()
        {
            // Act
            var okObjectResult = await _employeesController.Index("");

            // Assert
            Assert.IsType<OkObjectResult>(okObjectResult);
        }
        [Fact]
        public async Task IndexStrong_ReturnsAllItems()
        {
            // Act
            var okResult = await _employeesController.IndexStrong("");
            var okObjectResult = okResult.Result as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<Employee>>(okObjectResult.Value);
            Assert.Equal(3, items.Count);
        }
        [Fact]
        public async Task Index_ReturnsAllItems()
        {
            // Act
            var okObjectResult = await _employeesController.Index("") as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<Employee>>(okObjectResult.Value);
            Assert.Equal(3, items.Count());
        }
        [Fact]
        public async Task Index_ReturnsViewResult()
        {
            // Act
            var viewResult = await _homeController.Index();

            // Assert
            Assert.IsType<ViewResult>(viewResult);
        }
        [Fact]
        public async Task Index_ReturnsIEnumerableStormSessionViewModel()
        {
            // Act
            var viewResult = await _homeController.Index() as ViewResult;

            // Assert
            Assert.IsAssignableFrom<IEnumerable<StormSessionViewModel>>(viewResult.ViewData.Model);
        }
        [Fact]
        public async Task Index_ReturnsListOfBrainstormSessions()
        {
            // Act
            var viewResult = await _homeController.Index() as ViewResult;
            var model = viewResult.ViewData.Model as IEnumerable<StormSessionViewModel>;

            // Assert
            Assert.Equal(2, model.Count());
        }
        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfBrainstormSessions()
        {
            // Arrange
            var mockRepo_old = new Mock<IBrainstormSessionRepository>();
            mockRepo_old.Setup(repo => repo.ListAsync())
                .ReturnsAsync(GetTestSessions());
            var controller_old = new HomeController(mockRepo_old.Object);
            /*
            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(repo => repo.ListAsync())
                .ReturnsAsync(GetTestEmployees());
            var controller = new EmployeesController(mockRepo.Object);
            */
            // Act
            var result_old = await controller_old.Index();
            //var result = await controller.Index("");
            //IActionResult result2 = await controller.Index("");

            // Assert
            var viewResult_old = Assert.IsType<ViewResult>(result_old);
            var model_old = Assert.IsAssignableFrom<IEnumerable<StormSessionViewModel>>(
                viewResult_old.ViewData.Model);
            Assert.Equal(2, model_old.Count());
            /*
            OkObjectResult okObjectResult2 = Assert.IsType<OkObjectResult>(result2);
            List<Employee> model2 = Assert.IsType<List<Employee>>(okObjectResult2.Value);
            Assert.Equal(2, model2.Count);
            
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            //var model = Assert.IsAssignableFrom<List<Employee>>(okObjectResult.DeclaredType);
            var model = Assert.IsType<List<Employee>>(okObjectResult.Value);
            Assert.Equal(2, model.Count());
            */
            //Arrange
            /*var expected = "test";
            var controllerR = new EmployeesController(mockRepo.Object);

            //Act
            var actionResult = await controllerR.Index("");

            //Assert
            var okObjectResult = actionResult as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var model = okObjectResult.Value as List<Employee>;
            Assert.NotNull(model);
            
            var actual = model.Description;
            Assert.Equal(expected, actual);
            */
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
        public static List<Employee> GetTestEmployees()
        {
            var items = new List<Employee>
            {
                new Employee() { ID = 1001, Name = "Orange Juice", Group="Orange Tree", SalaryBase = 5.00M },
                new Employee() { ID = 1002, Name = "Diary Milk", Group="Cow", SalaryBase = 4.00M },
                new Employee() { ID = 1003, Name = "Frozen Pizza", Group="Uncle Mickey", SalaryBase = 12.00M }
            };
            return items;
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
