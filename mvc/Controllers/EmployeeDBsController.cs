using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MD.Salary.ConsoleApp.Models;
using MD.Salary.WebMvc.Models;

namespace WebMvc.Controllers
{
    public class EmployeeDBsController : Controller
    {
        private readonly EmployeeContext _context;

        public EmployeeDBsController(EmployeeContext context)
        {
            _context = context;
        }

        // GET: EmployeeDBs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }

        // GET: EmployeeDBs/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeDB = await _context.Employees
                .FirstOrDefaultAsync(m => m.ID == id);
            if (employeeDB == null)
            {
                return NotFound();
            }

            return View(employeeDB);
        }

        // GET: EmployeeDBs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EmployeeDBs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,HireDate,Group,SalaryBase,SuperiorID")] EmployeeDB employeeDB)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employeeDB);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employeeDB);
        }

        // GET: EmployeeDBs/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeDB = await _context.Employees.FindAsync(id);
            if (employeeDB == null)
            {
                return NotFound();
            }
            return View(employeeDB);
        }

        // POST: EmployeeDBs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,Name,HireDate,Group,SalaryBase,SuperiorID")] EmployeeDB employeeDB)
        {
            if (id != employeeDB.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employeeDB);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeDBExists(employeeDB.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employeeDB);
        }

        // GET: EmployeeDBs/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeDB = await _context.Employees
                .FirstOrDefaultAsync(m => m.ID == id);
            if (employeeDB == null)
            {
                return NotFound();
            }

            return View(employeeDB);
        }

        // POST: EmployeeDBs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var employeeDB = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employeeDB);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeDBExists(long id)
        {
            return _context.Employees.Any(e => e.ID == id);
        }
    }
}
