using MD.Salary.WebApi.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static MD.Salary.WebApi.Tests.UnitTests.Asyncs;

namespace MD.Salary.WebApi.Tests.UnitTests.Fake
{
    public class Index : EmployeesControllerTests
    {
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
