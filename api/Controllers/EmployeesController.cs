using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MD.Salary.WebApi.Models;
using MD.Salary.WebApi.Application;

namespace MD.Salary.WebApi.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeContext _context;

        public EmployeesController(EmployeeContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> Index(string searchString)
        {
            var items = from emp in _context.Employees select emp;
            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Name.Contains(searchString));
            }
            return await items.ToListAsync();
        }
        // GET: api/Employees/Salary
        [HttpGet("salary")]
        public async Task<ActionResult<decimal>> IndexSalary(long salaryDate)
        {
            await _context.Employees.ToListAsync();
            WebApiProgram.GetSalaryFromContext(_context.Employees, salaryDate);
            var total = Math.Round(WebApiProgram.SalaryCache.GetSum());

            return total;
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(long id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return employee;
        }
        // GET: api/Employees/5/Salary
        [HttpGet("{id}/salary")]
        public async Task<ActionResult<decimal>> GetEmployeeSalary(long id, long salaryDate)
        {
            await _context.Employees.ToListAsync();
            List<EmployeeFull> employeeList = WebApiProgram.GetSalaryFromContext(_context.Employees, salaryDate);
            var employee = employeeList.FirstOrDefault(emp => emp.ID == id);
            if (employee == null)
            {
                return NotFound();
            }
            return WebApiProgram.GetSalary(employee);
        }
        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee item)
        {
            _context.Employees.Add(item);
            await _context.SaveChangesAsync();

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

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(long id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}