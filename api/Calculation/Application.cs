using System;
using System.Collections.Generic;
using MD.Salary.WebApi.Models;
using MD.Salary.WebApi.Core.Models;
using System.Linq;

namespace MD.Salary.WebApi.Application
{
    public class WebApiProgram
    {
        public Tuple<List<Tuple<long, string, decimal>>, EmployeeFull> GetSubordinate(List<Employee> employeesDB, long salaryDate, long id)
        {
            Employees employees = GetSalaryFromDB(employeesDB, ConvertDateFormLong(salaryDate));
            EmployeeFull employee = employees.Items.FirstOrDefault(emp => emp.ID == id);
            List<Tuple<long, string, decimal>> salary = null;
            if (employee != null)
            {
                salary = employee.SubordinateList.Select(sub => new Tuple<long, string, decimal>
                (
                    sub.ID,
                    sub.Name,
                    employees.GetSalaryEmployee(sub.ID)
                )).ToList();
            }
            return new Tuple<List<Tuple<long, string, decimal>>, EmployeeFull>(salary, employee);
        }

        public Tuple<decimal, EmployeeFull> GetSalary(List<Employee> employeesDB, long salaryDate, long id = 0)
        {
            Employees employees = GetSalaryFromDB(employeesDB, ConvertDateFormLong(salaryDate));
            EmployeeFull employee = employees.Items.FirstOrDefault(emp => emp.ID == id);
            decimal salary;
            if (employee == null)
            {
                salary = employees.GetSalaryTotal();
            }
            else
            {
                salary = employees.GetSalaryEmployee(employee.ID);
            }
            return new Tuple<decimal, EmployeeFull>(salary, employee);
        }

        public Employees GetSalaryFromDB(List<Employee> employeesDB, DateTime salaryDate)
        {
            var employees = new Employees(employeesDB);
            employees.CalculateSubordinate();
            employees.CalculateSalary(salaryDate);
            return employees;
        }
        public DateTime ConvertDateFormLong(long salaryDate)
        {
            return DateTimeOffset.FromUnixTimeSeconds(salaryDate).UtcDateTime;
        }
    }
}
