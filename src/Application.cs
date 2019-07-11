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
            DateTime salaryDate;
            salaryDate = DateTime.Today;
            //salaryDate = DateTime.Parse("17/1/7");
            foreach (var employee in employeeList) employee.GetSalary(salaryDate);
            foreach (var employee in employeeList) WriteSalary(employee);
            Console.WriteLine("Итого = " + Round(SalaryCache.GetSum()));
            Console.Read();
        }
        public static void WriteSalary(Employee employee)
        {
            Console.WriteLine(employee.ID + " " + employee.Name + " " + employee.Group + " " +
                employee.HireDate.ToString("dd MMMM yyyy") + " зп = " + Round(SalaryCache.GetValue(employee.ID)));
        }
        public static MemoizationCache SalaryCache = new MemoizationCache();
    }
}
