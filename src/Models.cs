using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;
using MD.Salary.ConsoleApp.Application;

namespace MD.Salary.ConsoleApp.Models
{
    public enum Group { Employee, Manager, Salesman }
    public class EmployeeDB
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public DateTime HireDate { get; set; }
        public Group Group { get; set; }
        public decimal SalaryBase { get; set; }
        public long SuperiorID { get; set; }
    }
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
            HireDate = employeeDB.HireDate;
            Group = employeeDB.Group;
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
            Program.SalaryCache.Add(ID, salary);
            return salary;
        }
    }

    public class Calculator
    {
        public decimal SubordinateRate { get; set; }
        public decimal UnitRate { get; set; } = 100;
        public Calculator() { }
        public Calculator(decimal subordinateRate)
        {
            SubordinateRate = subordinateRate / UnitRate;
        }
        public decimal GetSalaryDirect(List<Employee> subList, DateTime salaryDate)
        {
            return subList.Sum(emp => Program.SalaryCache.ContainsKey(emp.ID) ? 
                                      Program.SalaryCache.GetValue(emp.ID) : emp.GetSalary(salaryDate));
        }
    }
    public class CalculatorPersonal : Calculator
    {
        public CalculatorPersonal(decimal experienceRate, decimal limitRate) : base()
        {
            ExperienceRate = experienceRate / UnitRate;
            LimitRate = limitRate / UnitRate;
        }
        private decimal ExperienceRate { get; set; }
        private decimal LimitRate { get; set; }
        private int GetExperience(DateTime salaryDate, DateTime hireDate)
        {
            var exp = salaryDate.Year - hireDate.Year;
            if (hireDate.Date > salaryDate.AddYears(-exp)) exp--;
            return exp;
        }
        public decimal GetSalary(decimal salaryBase, DateTime hireDate, DateTime salaryDate)
        {
            return (Min(ExperienceRate * GetExperience(salaryDate, hireDate), LimitRate) + 1) * salaryBase;
        }
    }
    public interface ICalculatorSub
    {
        decimal GetSalary(List<Employee> subList, DateTime salaryDate);
    }
    class ICalculatorSubEmployee : Calculator, ICalculatorSub
    {
        public ICalculatorSubEmployee(decimal subordinateRate) : base(subordinateRate) { }
        public decimal GetSalary(List<Employee> subList, DateTime salaryDate)
        {
            return 0;
        }
    }
    class ICalculatorSubManager : Calculator, ICalculatorSub
    {
        public ICalculatorSubManager(decimal subordinateRate) : base(subordinateRate) { }
        public decimal GetSalary(List<Employee> subList, DateTime salaryDate)
        {
            return SubordinateRate * GetSalaryDirect(subList, salaryDate);
        }
    }
    class ICalculatorSubSalesman : Calculator, ICalculatorSub
    {
        public ICalculatorSubSalesman(decimal subordinateRate) : base(subordinateRate) { }
        public decimal GetSalary(List<Employee> subList, DateTime salaryDate)
        {
            return SubordinateRate * (GetSalaryDirect(subList, salaryDate) + GetSalaryIndirect(subList, salaryDate));
        }
        private decimal GetSalaryIndirect(List<Employee> subList, DateTime salaryDate)
        {
            return subList.Sum(emp => GetSalaryDirect(emp.SubordinateList, salaryDate));
        }
    }

    public class Factory
    {
        public struct Calculators
        {
            public CalculatorPersonal Personal;
            public ICalculatorSub Sub;
        }
        private Dictionary<Group, Calculators> SalaryDictionary { get; set; }
        public Factory()
        {
            SalaryDictionary = new Dictionary<Group, Calculators> {
                {Group.Employee, new Calculators {Sub = new ICalculatorSubEmployee(0), Personal = new CalculatorPersonal(3, 30) } },
                {Group.Manager, new Calculators {Sub = new ICalculatorSubManager(0.5m), Personal = new CalculatorPersonal(5, 40) } },
                {Group.Salesman, new Calculators {Sub = new ICalculatorSubSalesman(0.3m), Personal = new CalculatorPersonal(1, 35) } }
            };
        }
        public Calculators GetCalculator(Group group)
        {
            return SalaryDictionary[group];
        }
    }
}

