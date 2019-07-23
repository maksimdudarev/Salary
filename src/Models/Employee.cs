using System;
using System.Collections.Generic;
using System.Linq;
using MD.Salary.ConsoleApp.Application;

namespace MD.Salary.ConsoleApp.Models
{
    public class Employee
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public DateTime HireDate { get; set; }
        public Group Group { get; set; }
        public decimal SalaryBase { get; set; }
        public long SuperiorID { get; set; }
        public List<Employee> SubordinateList { get; set; }
        public ICalculatorSub SalarySub { get; set; }
        public CalculatorPersonal SalaryPersonal { get; set; }
        public Employee(EmployeeDB employeeDB)
        {
            ID = employeeDB.ID;
            Name = employeeDB.Name;
            HireDate = DateTimeOffset.FromUnixTimeSeconds(employeeDB.HireDate).UtcDateTime;
            Enum.TryParse(employeeDB.Group, out Group group);
            Group = group;
            SalaryBase = employeeDB.SalaryBase;
            SuperiorID = employeeDB.SuperiorID;
            var calculators = new Factory().GetCalculator(Group);
            SalaryPersonal = calculators.Personal;
            SalarySub = calculators.Sub;
        }
        public void CalculateSubordinate(List<Employee> employeeList)
        {
            List<long> subordinateID = employeeList.Where(emp => emp.SuperiorID == ID).Select(emp => emp.ID).ToList();
            SubordinateList = employeeList.Where(emp => subordinateID.Contains(emp.ID)).ToList();
        }
        public decimal GetSalary(DateTime salaryDate)
        {
            decimal salary = SalaryPersonal.GetSalary(SalaryBase, HireDate, salaryDate) + SalarySub.GetSalary(SubordinateList, salaryDate);
            ConsoleAppProgram.SalaryCache.Add(ID, salary);
            return salary;
        }
    }
}
