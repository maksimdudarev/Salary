using System;
using System.Collections.Generic;
using MD.Salary.WebApi.Models;
using MD.Salary.WebApi.Core.Models;
using System.Linq;

namespace MD.Salary.WebApi.Application
{
    public class WebApiProgram
    {
        public Employees Employees { get; set; }
        public EmployeeFull EmployeeById { get; set; }
        public WebApiProgram(List<Employee> items, long salaryDate)
        {
            Employees = new Employees(items);
            Employees.CalculateSubordinate();
            Employees.CalculateSalary(DateTimeOffset.FromUnixTimeSeconds(salaryDate).UtcDateTime);
        }
        public WebApiProgram(List<Employee> items, long salaryDate, long id) : this(items, salaryDate)
        {
            EmployeeById = Employees.Items.FirstOrDefault(emp => emp.ID == id);
        }

        public decimal? GetSalaryById()
        {
            decimal? salary = null;
            if (EmployeeById != null) salary = Employees.GetSalaryEmployee(EmployeeById.ID);
            return salary;
        }

        public List<Tuple<long, decimal>> GetSubordinate()
        {
            List<Tuple<long, decimal>> salary = null;
            if (EmployeeById != null)
            {
                salary = EmployeeById.SubordinateList.Select(sub => new Tuple<long, decimal> (
                    sub.ID,
                    Employees.GetSalaryEmployee(sub.ID)
                )).ToList();
            }
            return salary;
        }
    }
}
