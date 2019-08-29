using System;
using System.Collections.Generic;
using static System.Math;
using MD.Salary.WebApi.Models;
using MD.Salary.WebApi.Utilities;
using MD.Salary.WebApi.Core.Models;

namespace MD.Salary.WebApi.Application
{
    public class Employees
    {
        public List<EmployeeFull> Items { get; set; } = new List<EmployeeFull>();
        public Employees(List<Employee> employeesDB)
        {
            foreach (var employeeDB in employeesDB) Items.Add(new EmployeeFull(employeeDB));
        }
        public void CalculateSubordinate()
        {
            foreach (var employee in Items) employee.CalculateSubordinate(Items);
        }
        public void CalculateSalary(DateTime salaryDate)
        {
            foreach (var employee in Items) employee.GetSalary(salaryDate, SalaryCache);
        }
        public decimal GetSalaryEmployee(long id)
        {
            return Round(SalaryCache.GetValue(id));
        }
        public decimal GetSalaryTotal()
        {
            return Round(SalaryCache.GetSum());
        }
        public MemoizationCache SalaryCache = new MemoizationCache();
    }
}
