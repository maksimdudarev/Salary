using System;
using System.Collections.Generic;
using static System.Math;
using MD.Salary.WebApi.Models;
using MD.Salary.WebApi.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MD.Salary.WebApi.Application
{
    public static class WebApiProgram
    {
        public static List<EmployeeFull> GetSalaryFromContext(DbSet<Employee> EmployeeDbset, long salaryDate)
        {
            List<EmployeeFull> employeeList = GetEmployeeListFromDB(EmployeeDbset);
            employeeList = CalculateSalary(employeeList, DateTimeOffset.FromUnixTimeSeconds(salaryDate).UtcDateTime);
            return employeeList;
        }
        public static List<EmployeeFull> CalculateSalary(List<EmployeeFull> employeeList, DateTime salaryDate)
        {
            foreach (var employee in employeeList) employee.CalculateSubordinate(employeeList);
            foreach (var employee in employeeList) employee.GetSalary(salaryDate);
            return employeeList;
        }
        public static List<EmployeeFull> GetEmployeeListFromDB(DbSet<Employee> EmployeeDbset)
        {
            var employeeList = new List<EmployeeFull> { };
            foreach (var employeeDB in EmployeeDbset) employeeList.Add(new EmployeeFull(employeeDB));
            return employeeList;
        }
        public static decimal GetSalary(EmployeeFull employee)
        {
            return Round(SalaryCache.GetValue(employee.ID));
        }
        public static decimal GetSalaryTotal()
        {
            return Round(SalaryCache.GetSum());
        }
        public static MemoizationCache SalaryCache = new MemoizationCache();
    }
}
