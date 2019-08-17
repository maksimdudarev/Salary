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
using MD.Salary.WebApi.Tests;
using Xunit;
using WebApiForTests.Contracts;
using WebApiForTests.Controllers;
using WebApiForTests.Model;
using WebApiForTests.Tests;

namespace TestingControllersSample.Tests.UnitTests
{
    public class HomeControllerTests
    {
        ShoppingCartController _controller;
        IShoppingCartService _service;
        EmployeesController _controllerE;
        IEmployeeRepository _serviceE;
        HomeController _homeController;
        Mock<IBrainstormSessionRepository> _repositoryBrainstormSession;
        EmployeesController _employeesController;
        Mock<IEmployeeRepository> _employeeRepository;

        public HomeControllerTests()
        {
            _service = new ShoppingCartServiceFake();
            _controller = new ShoppingCartController(_service);
            _serviceE = new EmployeeRepositoryFake();
            _controllerE = new EmployeesController(_serviceE);
            _repositoryBrainstormSession = new Mock<IBrainstormSessionRepository>();
            _repositoryBrainstormSession.
                Setup(repo => repo.ListAsync()).ReturnsAsync(GetTestSessions());
            _homeController = new HomeController(_repositoryBrainstormSession.Object);
            _employeeRepository = new Mock<IEmployeeRepository>();
            _employeeRepository.
                Setup(repo => repo.ListAsync()).ReturnsAsync(GetTestEmployees());
            _employeesController = new EmployeesController(_employeeRepository.Object);
        }

        [Fact]
        public void IndexSync_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = _controllerE.IndexSync();

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }
        [Fact]
        public void IndexSync_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = _controllerE.IndexSync().Result as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<Employee>>(okResult.Value);
            Assert.Equal(3, items.Count);
        }
        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = _controller.Get();

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = _controller.Get().Result as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<ShoppingItem>>(okResult.Value);
            Assert.Equal(3, items.Count);
        }
        #region snippet_Index_ReturnsAViewResult_WithAListOfBrainstormSessions
        [Fact]
        public async Task Index_ReturnsOkObjectResult()
        {
            // Act
            var okObjectResult = await _employeesController.Index("");

            // Assert
            Assert.IsType<OkObjectResult>(okObjectResult);
        }
        [Fact]
        public async Task Index_ReturnsAllItems()
        {
            // Act
            var okResult = _controller.Get().Result as OkObjectResult;

            var viewResult = await _homeController.Index() as ViewResult;
            var okObjectResult = await _employeesController.Index("") as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<ShoppingItem>>(okResult.Value);
            Assert.Equal(3, items.Count);

            var items2 = Assert.IsAssignableFrom<IEnumerable<StormSessionViewModel>>(okObjectResult.Value);
            Assert.Equal(2, items2.Count());
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

        #region snippet_ShoppingCartOther
        [Fact]
        public void GetById_UnknownGuidPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = _controller.Get(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetById_ExistingGuidPassed_ReturnsOkResult()
        {
            // Arrange
            var testGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

            // Act
            var okResult = _controller.Get(testGuid);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetById_ExistingGuidPassed_ReturnsRightItem()
        {
            // Arrange
            var testGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

            // Act
            var okResult = _controller.Get(testGuid).Result as OkObjectResult;

            // Assert
            Assert.IsType<ShoppingItem>(okResult.Value);
            Assert.Equal(testGuid, (okResult.Value as ShoppingItem).Id);
        }

        [Fact]
        public void Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var nameMissingItem = new ShoppingItem()
            {
                Manufacturer = "Guinness",
                Price = 12.00M
            };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var badResponse = _controller.Post(nameMissingItem);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }


        [Fact]
        public void Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            ShoppingItem testItem = new ShoppingItem()
            {
                Name = "Guinness Original 6 Pack",
                Manufacturer = "Guinness",
                Price = 12.00M
            };

            // Act
            var createdResponse = _controller.Post(testItem);

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }


        [Fact]
        public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            var testItem = new ShoppingItem()
            {
                Name = "Guinness Original 6 Pack",
                Manufacturer = "Guinness",
                Price = 12.00M
            };

            // Act
            var createdResponse = _controller.Post(testItem) as CreatedAtActionResult;
            var item = createdResponse.Value as ShoppingItem;

            // Assert
            Assert.IsType<ShoppingItem>(item);
            Assert.Equal("Guinness Original 6 Pack", item.Name);
        }

        [Fact]
        public void Remove_NotExistingGuidPassed_ReturnsNotFoundResponse()
        {
            // Arrange
            var notExistingGuid = Guid.NewGuid();

            // Act
            var badResponse = _controller.Remove(notExistingGuid);

            // Assert
            Assert.IsType<NotFoundResult>(badResponse);
        }

        [Fact]
        public void Remove_ExistingGuidPassed_ReturnsOkResult()
        {
            // Arrange
            var existingGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

            // Act
            var okResponse = _controller.Remove(existingGuid);

            // Assert
            Assert.IsType<OkResult>(okResponse);
        }
        [Fact]
        public void Remove_ExistingGuidPassed_RemovesOneItem()
        {
            // Arrange
            var existingGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

            // Act
            var okResponse = _controller.Remove(existingGuid);

            // Assert
            Assert.Equal(2, _service.GetAllItems().Count());
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
