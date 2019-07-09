using System;
using System.Collections.Generic;
using static System.Math;
using MD.Salary.Database;
using MD.Salary.Model;
using MD.Salary.Utilities;
using System.Data.SQLite;

namespace MD.Salary.Application
{
    static class Program
    {
        static void Main(string[] args)
        {
            SQLiteConnection sqlite_conn;
            sqlite_conn = DBConnection.CreateConnection();
            DBConnection.CreateTable(sqlite_conn);
            DBConnection.InsertData(sqlite_conn);
            DBConnection.ReadData(sqlite_conn);

            var employeeList = new List<Employee> {
                new Employee(1, "Смит", DateTime.Parse("5/5/5"), Groups.Employee, 15, new List<int> { }),
                new Employee(2, "Гейтс", DateTime.Parse("7/7/7"), Groups.Manager, 30, new List<int> {1, 6}),
                new Employee(3, "Трамп", DateTime.Parse("11/12/13"), Groups.Salesman, 35, new List<int> {2, 4, 8}),
                new Employee(4, "Паркер", DateTime.Parse("8/4/2"), Groups.Employee, 20, new List<int> { }),
                new Employee(5, "Морган", DateTime.Parse("15/10/15"), Groups.Manager, 40, new List<int> {3}),
                new Employee(6, "Хьюз", DateTime.Parse("7/5/75"), Groups.Salesman, 25, new List<int> {7}),
                new Employee(7, "МакФлай", DateTime.Parse("31/1/13"), Groups.Employee, 10, new List<int> { }),
                new Employee(8, "Уиллис", DateTime.Parse("1/1/1"), Groups.Manager, 45, new List<int> { })
            };
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
