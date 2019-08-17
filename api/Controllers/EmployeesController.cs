using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MD.Salary.WebApi.Models;
using MD.Salary.WebApi.Application;
using MD.Salary.WebApi.Core.Interfaces;
using MD.Salary.WebApi.Core.Models;

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
        public async Task<ActionResult<List<Employee>>> IndexAsync()
        {
            var items = await _repository.GetAllItemsAsync();
            return Ok(items);
        }
        [HttpGet]
        public async Task<ActionResult<List<Employee>>> IndexStrong(string searchString)
        {
            var items = await _repository.ListBySearhstringAsync(searchString);
            return Ok(items);
        }
        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            var items = await _repository.ListBySearhstringAsync(searchString);
            return Ok(items);
        }
        // GET: api/Employees/Salary
        [HttpGet("salary")]
        public async Task<IActionResult> IndexSalary(long salaryDate)
        {
            var items = await _repository.ListAsync();
            var program = new WebApiProgram();
            program.GetSalaryFromContext(items, salaryDate);
            return Ok(program.GetSalaryTotal());
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(long id)
        {
            var item = await _repository.GetByIdAsync(id);
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
            var items = await _repository.ListAsync();
            var program = new WebApiProgram();
            List<EmployeeFull> employeeList = program.GetSalaryFromContext(items, salaryDate);
            var item = employeeList.FirstOrDefault(emp => emp.ID == id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(program.GetSalary(item));
        }
        // POST: api/Employees
        [HttpPost]
        public async Task<IActionResult> PostEmployee(Employee item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _repository.AddAsync(item);
            //return RedirectToAction(nameof(Index));
            return CreatedAtAction(nameof(GetEmployee), new { id = item.ID }, item);
        }
        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(long id, Employee item)
        {
            if (id != item.ID)
            {
                return BadRequest();
            }
            await _repository.UpdateAsync(item);
            return RedirectToAction(nameof(Index));
        }
        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            await _repository.DeleteAsync(item);
            return RedirectToAction(nameof(Index));
        }
    }
}