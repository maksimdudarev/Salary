using System;
using System.Collections.Generic;
using static System.Math;
using MD.Salary.ConsoleApp.Models;
using MD.Salary.ConsoleApp.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MD.Salary.ConsoleApp.Application
{
    public static class ConsoleAppProgram
    {
        static void Main(string[] args)
        {
            List<EmployeeFull> employeeList;
            using (var db = new EmployeeFullContext()) employeeList = GetEmployeeListFromDB(db.Employees);
            DateTime salaryDate = InputDate();
            foreach (var employee in employeeList) employee.CalculateSubordinate(employeeList);
            foreach (var employee in employeeList) employee.GetSalary(salaryDate);
            foreach (var employee in employeeList) WriteSalary(employee);
            Console.WriteLine($"\nTotal = {Round(SalaryCache.GetSum())} Date = {salaryDate}");
            Console.ReadLine();
        }
        public static List<EmployeeFull> GetEmployeeListFromDB(DbSet<Employee> EmployeeDbset)
        {
            var employeeList = new List<EmployeeFull> { };
            foreach (var employeeDB in EmployeeDbset) employeeList.Add(new EmployeeFull(employeeDB));
            return employeeList;
        }
        public static DateTime InputDate()
        {
            string inputDate = "17/1/7";
            Console.WriteLine($"Input date (for ex., {inputDate}): ");
            inputDate = Console.ReadLine();
            if (!DateTime.TryParse(inputDate, out DateTime salaryDate)) salaryDate = DateTime.Today;
            return salaryDate;
        }
        public static decimal GetSalary(EmployeeFull employee)
        {
            return Round(SalaryCache.GetValue(employee.ID));
        }
        public static void WriteSalary(EmployeeFull employee)
        {
            Console.WriteLine($"{employee.ID} {employee.Name} {employee.Group} " +
                $"{employee.HireDate.ToString("dd MMMM yyyy")} salary = {GetSalary(employee)}");
        }
        public static MemoizationCache SalaryCache = new MemoizationCache();
    }
}
