using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MD.Salary.WebApi.Models;
using MD.Salary.ConsoleApp.Models;

namespace MD.Salary.WebApi.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext _context;

        public EmployeeController(EmployeeContext context)
        {
            _context = context;

            if (_context.EmployeeItems.Count() == 0)
            {
                // Create a new EmployeeItem if collection is empty,
                // which means you can't delete all EmployeeItems.
                _context.EmployeeItems.Add(new EmployeeDB { Name = "Item1" });
                _context.SaveChanges();
            }
        }
        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDB>>> GetEmployeeItems()
        {
            return await _context.EmployeeItems.ToListAsync();
        }

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDB>> GetEmployeeItem(long id)
        {
            var employeeItem = await _context.EmployeeItems.FindAsync(id);

            if (employeeItem == null)
            {
                return NotFound();
            }

            return employeeItem;
        }
        // POST: api/Employee
        [HttpPost]
        public async Task<ActionResult<EmployeeDB>> PostEmployeeItem(EmployeeDB item)
        {
            _context.EmployeeItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployeeItem), new { id = item.ID }, item);
        }
        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeItem(long id, EmployeeDB item)
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
        public async Task<IActionResult> DeleteEmployeeItem(long id)
        {
            var employeeItem = await _context.EmployeeItems.FindAsync(id);

            if (employeeItem == null)
            {
                return NotFound();
            }

            _context.EmployeeItems.Remove(employeeItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}