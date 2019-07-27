using System;
using System.Collections.Generic;
using static System.Math;
using MD.Salary.ConsoleApp.Models;
using MD.Salary.ConsoleApp.Utilities;

namespace MD.Salary.ConsoleApp.Application
{
    public static class ConsoleAppProgram
    {
        static void Main(string[] args)
        {
            var employeeList = new List<Employee> { };
            using (var db = new EmployeeContext())
            {
                foreach (var employeeDB in db.Employees) employeeList.Add(new Employee(employeeDB));
            }
            foreach (var employee in employeeList) employee.CalculateSubordinate(employeeList);
            DateTime salaryDate = InputDate();
            foreach (var employee in employeeList) employee.GetSalary(salaryDate);
            foreach (var employee in employeeList) WriteSalary(employee);
            Console.WriteLine("\nTotal = " + Round(SalaryCache.GetSum()) + " Date = " + salaryDate);
            Console.ReadLine();
        }
        public static DateTime InputDate()
        {
            string inputDate = "17/1/7";
            Console.WriteLine("Input date (for ex., " + inputDate + "): ");
            inputDate = Console.ReadLine();
            if (!DateTime.TryParse(inputDate, out DateTime salaryDate)) salaryDate = DateTime.Today;
            return salaryDate;
        }
        public static void WriteSalary(Employee employee)
        {
            Console.WriteLine(employee.ID + " " + employee.Name + " " + employee.Group + " " +
                employee.HireDate.ToString("dd MMMM yyyy") + " salary = " + Round(SalaryCache.GetValue(employee.ID)));
        }
        public static MemoizationCache SalaryCache = new MemoizationCache();
    }
}
