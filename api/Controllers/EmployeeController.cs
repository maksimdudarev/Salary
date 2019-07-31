using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MD.Salary.ConsoleApp.Models;

namespace MD.Salary.WebApi.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly Models.EmployeeContext _context;

        public EmployeeController(Models.EmployeeContext context)
        {
            _context = context;

            if (_context.Employees.Count() == 0)
            {
                // Create a new Employee if collection is empty,
                // which means you can't delete all Employees.
                _context.Employees.Add(new EmployeeDB { Name = "Item1" });
                _context.SaveChanges();
            }
        }
        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDB>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDB>> GetEmployee(long id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }
        // POST: api/Employee
        [HttpPost]
        public async Task<ActionResult<EmployeeDB>> PostEmployee(EmployeeDB item)
        {
            _context.Employees.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = item.ID }, item);
        }
        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(long id, EmployeeDB item)
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