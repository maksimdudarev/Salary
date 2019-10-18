using MD.Salary.WebApi.Core.Interfaces;
using MD.Salary.WebApi.Controllers;
using Moq;

namespace MD.Salary.WebApi.Tests.UnitTests.Moq
{
    public class EmployeesControllerTests
    {
        public Mock<IEmployeeRepository> _repository;
        public EmployeesController _controller;

        public EmployeesControllerTests()
        {
            _repository = new Mock<IEmployeeRepository>();
            _controller = new EmployeesController(_repository.Object);
        }
    }
}
