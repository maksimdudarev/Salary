using System;
using System.Collections.Generic;
using static System.Math;
using MD.Salary.ConsoleApp.Models;
using MD.Salary.WebApi.Application;
using MD.Salary.WebApi.Models;

namespace MD.Salary.ConsoleApp.Application
{
    public static class ConsoleAppProgram
    {
        static void Main(string[] args)
        {
            List<EmployeeFull> employeeList;
            using (var db = new EmployeeFullContext()) employeeList = WebApiProgram.GetEmployeeListFromDB(db.Employees);
            DateTime salaryDate = InputDate();
            employeeList = WebApiProgram.CalculateSalary(employeeList, salaryDate);
            foreach (var employee in employeeList) WriteSalary(employee);
            Console.WriteLine($"\nTotal = {Round(WebApiProgram.SalaryCache.GetSum())} Date = {salaryDate}");
            Console.ReadLine();
        }
        public static DateTime InputDate()
        {
            string inputDate = "17/1/7"; // timestamp = 1168981200
            Console.WriteLine($"Input date (for ex., {inputDate}): ");
            inputDate = Console.ReadLine();
            if (!DateTime.TryParse(inputDate, out DateTime salaryDate)) salaryDate = DateTime.Today;
            return salaryDate;
        }
        public static void WriteSalary(EmployeeFull employee)
        {
            Console.WriteLine($"{employee.ID} {employee.Name} {employee.Group} " +
                $"{employee.HireDate.ToString("dd MMMM yyyy")} salary = {WebApiProgram.GetSalary(employee)}");
        }
    }
}
