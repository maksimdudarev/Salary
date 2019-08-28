using MD.Salary.WebApi.Controllers;
using MD.Salary.WebApi.Core.Interfaces;
using static MD.Salary.WebApi.Tests.UnitTests.Constants;

namespace MD.Salary.WebApi.Tests.UnitTests.Fake
{
    public class EmployeesControllerTests
    {
        public EmployeesController _controller;
        public IEmployeeRepository _service;

        public EmployeesControllerTests()
        {
            _service = new EmployeeRepositoryFake(GetTestEmployees());
            _controller = new EmployeesController(_service);
        }
    }
}
