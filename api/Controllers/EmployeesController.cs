using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MD.Salary.ConsoleApp.Models;
using MD.Salary.ConsoleApp.Application;
using MD.Salary.WebApi.Models;

namespace MD.Salary.WebApi.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeContext _context;

        public EmployeesController(EmployeeContext context)
        {
            _context = context;
        }

        // GET: api/Employee
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

        // GET: api/Employee/5
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
        // GET: api/Employee/Salary/5
        [HttpGet("salary/{id}")]
        public async Task<ActionResult<decimal>> GetSalary(long id, long salaryDate)
        {
            List<EmployeeFull> employeeList = ConsoleAppProgram.GetEmployeeListFromDB(_context.Employees);
            foreach (var employee in employeeList) employee.CalculateSubordinate(employeeList);
            foreach (var employee in employeeList) employee.GetSalary(DateTimeOffset.FromUnixTimeSeconds(salaryDate).UtcDateTime);

            var employee2 = await _context.Employees.FindAsync(id);

            if (employee2 == null)
            {
                return NotFound();
            }

            return employee2.SalaryBase;
        }
        // POST: api/Employee
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee item)
        {
            _context.Employees.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = item.ID }, item);
        }
        // PUT: api/Employee/5
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
        // DELETE: api/Employee/5
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