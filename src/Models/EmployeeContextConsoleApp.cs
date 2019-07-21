using Microsoft.EntityFrameworkCore;
using MD.Salary.ConsoleApp.Application;

namespace MD.Salary.ConsoleApp.Models
{
    public class EmployeeContextConsoleApp : EmployeeContext
    {
        public EmployeeContextConsoleApp()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = " + ConsoleAppProgram.SalaryDBPathConsole);
        }
    }
}