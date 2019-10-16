using Moq;
using MD.Salary.WebApi.Core.Interfaces;
using MD.Salary.WebApi.Controllers;

namespace MD.Salary.WebApi.Tests.UnitTests.Moq
{
    public class EmployeesControllerTests
    {
        public Mock<ICombineRepository> _repository;
        public EmployeesController _controller;

        public EmployeesControllerTests()
        {
            _repository = new Mock<ICombineRepository>();
            _controller = new EmployeesController(_repository.Object);
        }
    }
}
