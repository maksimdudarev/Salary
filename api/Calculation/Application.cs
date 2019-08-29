using System;
using System.Collections.Generic;
using static System.Math;
using MD.Salary.WebApi.Models;
using MD.Salary.WebApi.Utilities;
using MD.Salary.WebApi.Core.Models;
using System.Linq;

namespace MD.Salary.WebApi.Application
{
    public class WebApiProgram
    {
        public List<Employee> GetSubs(long id, List<Employee> employeesDB)
        {
            return employeesDB;
        }

        public Tuple<decimal, EmployeeFull> GetSalaryForEndpoint(List<Employee> employeesDB, long salaryDate, long id = 0)
        {
            List<EmployeeFull> employees = GetSalaryFromDB(employeesDB, DateTimeOffset.FromUnixTimeSeconds(salaryDate).UtcDateTime);
            decimal salary;
            EmployeeFull employee = employees.FirstOrDefault(emp => emp.ID == id);
            if (employee == null)
            {
                salary = GetSalaryTotal();
            }
            else
            {
                salary = GetSalary(employee);
            }
            return new Tuple<decimal, EmployeeFull>(salary, employee);
        }

        public List<EmployeeFull> GetSalaryFromDB(List<Employee> employeesDB, DateTime salaryDate)
        {
            List<EmployeeFull> employees = GetEmployeesFromDB(employeesDB);
            employees = CalculateSubordinate(employees);
            employees = CalculateSalary(employees, salaryDate);
            return employees;
        }
        private List<EmployeeFull> CalculateSubordinate(List<EmployeeFull> employees)
        {
            foreach (var employee in employees) employee.CalculateSubordinate(employees);
            return employees;
        }
        private List<EmployeeFull> CalculateSalary(List<EmployeeFull> employees, DateTime salaryDate)
        {
            foreach (var employee in employees) employee.GetSalary(salaryDate, SalaryCache);
            return employees;
        }
        private List<EmployeeFull> GetEmployeesFromDB(List<Employee> employeesDB)
        {
            var employees = new List<EmployeeFull>();
            foreach (var employeeDB in employeesDB) employees.Add(new EmployeeFull(employeeDB));
            return employees;
        }
        public decimal GetSalary(EmployeeFull employee)
        {
            return Round(SalaryCache.GetValue(employee.ID));
        }
        public decimal GetSalaryTotal()
        {
            return Round(SalaryCache.GetSum());
        }
        public MemoizationCache SalaryCache = new MemoizationCache();
    }
}
