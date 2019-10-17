using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MD.Salary.WebApi.Application;
using MD.Salary.WebApi.Core.Interfaces;
using MD.Salary.WebApi.Core.Models;
using MD.Salary.WebApi.Middleware;

namespace MD.Salary.WebApi.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;

        public EmployeesController(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            var items = await _repository.EmployeeListBySearhstringAsync(searchString);
            return Ok(items);
        }

        // GET: api/Employees/Salary
        [HttpGet("salary")]
        public async Task<IActionResult> IndexSalary(long salaryDate)
        {
            var items = await _repository.EmployeeListBySearhstringAsync();
            var program = new WebApiProgram(items, salaryDate);
            return Ok(program.Employees.GetSalaryTotal());
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(long id)
        {
            var item = await _repository.GetEmployeeByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // GET: api/Employees/5/Salary
        [HttpGet("{id}/salary")]
        public async Task<IActionResult> GetEmployeeSalary(long id, long salaryDate)
        {
            var checkId = await _repository.CheckUserAsync(HttpContext, id);
            var checkRole = await _repository.CheckRoleAsync(HttpContext);
            if (!(checkId || checkRole))
            {
                return Unauthorized();
            }
            var items = await _repository.EmployeeListBySearhstringAsync();
            var program = new WebApiProgram(items, salaryDate, id);
            var salary = program.GetSalaryById();
            if (salary == null)
            {
                return NotFound();
            }
            return Ok(salary);
        }

        // GET: api/Employees/5/Subs
        [HttpGet("{id}/subs")]
        public async Task<IActionResult> GetEmployeeSubs(long id, long salaryDate)
        {
            var checkId = await _repository.CheckUserAsync(HttpContext, id);
            var checkRole = await _repository.CheckRoleAsync(HttpContext);
            if (!(checkId || checkRole))
            {
                return Unauthorized();
            }
            var items = await _repository.EmployeeListBySearhstringAsync();
            var program = new WebApiProgram(items, salaryDate, id);
            var salary = program.GetSubordinate();
            if (salary == null)
            {
                return NotFound();
            }
            return Ok(salary);
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _repository.AddEmployeeAsync(item);
            return CreatedAtAction(nameof(GetEmployee), new { id = item.UserId }, item);
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(long id, [FromBody] Employee item)
        {
            if (id != item.UserId)
            {
                return BadRequest();
            }
            await _repository.UpdateEmployeeAsync(item);
            return RedirectToAction(nameof(Index));
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(long id)
        {
            var item = await _repository.GetEmployeeByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            await _repository.DeleteEmployeeAsync(item);
            return RedirectToAction(nameof(Index));
        }
    }
}