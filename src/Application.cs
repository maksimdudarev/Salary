using System;
using System.Collections.Generic;
using static System.Math;
using MD.Salary.Database;
using MD.Salary.Model;
using MD.Salary.Utilities;

namespace MD.Salary.Application
{
    static class Program
    {
        static void Main(string[] args)
        {
            string tableName = "employees";
            List<EmployeeDB> employeeListDB = DataRetriever.GetData(Environment.CurrentDirectory + "/" + tableName + ".db", tableName);
            var employeeList = new List<Employee> { };
            foreach (var employeeDB in employeeListDB) employeeList.Add(new Employee(employeeDB));
            foreach (var employee in employeeList) employee.CalculateSubordinate(employeeList);
            DateTime salaryDate = InputData();
            foreach (var employee in employeeList) employee.GetSalary(salaryDate);
            foreach (var employee in employeeList) WriteSalary(employee);
            Console.WriteLine("\nTotal = " + Round(SalaryCache.GetSum()) + " Date = " + salaryDate);
            Console.ReadLine();
        }
        public static DateTime InputData()
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
