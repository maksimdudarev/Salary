using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MD.Salary.WebApi.Controllers;
using MD.Salary.WebApi.Core.Models;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;
using static MD.Salary.WebApi.Tests.UnitTests.Constants;

namespace MD.Salary.WebApi.Tests.UnitTests.Moq
{
    public class Index : EmployeesControllerTests
    {
        readonly EmployeesController _controller;

        public Index()
        {
            _repository.Setup(repo => repo.ListBySearhstringAsync("")).ReturnsAsync(GetTestEmployees());
            _controller = new EmployeesController(_repository.Object);
        }

        [Fact]
        public void WhenCalled_ReturnsOkObjectResult()
        {
            // Act
            var okObjectResult = GetAsyncActionResult(() => _controller.Index(""));

            // Assert
            Assert.IsType<OkObjectResult>(okObjectResult);
        }

        [Fact]
        public void WhenCalled_ReturnsAllItems()
        {
            // Act
            var okObjectResult = GetAsyncActionResult(() => _controller.Index("")) as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<Employee>>(okObjectResult.Value);
            Assert.Equal(3, items.Count());
        }
    }
}
