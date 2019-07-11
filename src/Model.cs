using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;
using MD.Salary.Application;

namespace MD.Salary.Model
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
        public ISalarySubCalculator SalarySub { get; set; }
        public SalaryPersonalCalculator SalaryPersonal { get; set; }
        public Employee(EmployeeDB employeeDB)
        {
            ID = employeeDB.ID;
            Name = employeeDB.Name;
            HireDate = employeeDB.HireDate;
            Group = employeeDB.Group;
            SalaryBase = employeeDB.SalaryBase;
            SuperiorID = employeeDB.SuperiorID;
            var salaryCalculators = new SalaryFactory().GetSalaryCalculator(Group);
            SalaryPersonal = salaryCalculators.SalaryPersonal;
            SalarySub = salaryCalculators.SalarySub;
        }
        public void CalculateSubordinate(List<Employee> employeeList)
        {
            List<long> subordinateID = employeeList.Where(emp => emp.SuperiorID == ID).Select(emp => emp.ID).ToList();
            SubordinateList = employeeList.Where(emp => subordinateID.Contains(emp.ID)).ToList();
        }
        public decimal GetSalary(DateTime? salaryDateOptional = null)
        {
            var salaryDate = salaryDateOptional ?? DateTime.Today;
            var salary = SalaryPersonal.GetSalary(SalaryBase, HireDate, salaryDate) + SalarySub.GetSalary(SubordinateList, salaryDate);
            Program.SalaryCache.Add(ID, salary);
            return salary;
        }
    }

    public class SalaryCalculator
    {
        public decimal ExperienceRate { get; set; }
        public decimal LimitRate { get; set; }
        public decimal SubordinateRate { get; set; }
        public SalaryCalculator(SalaryRates salaryRates)
        {
            decimal unitRate = 100;
            ExperienceRate = salaryRates.Experience / unitRate;
            LimitRate = salaryRates.Limit / unitRate;
            SubordinateRate = salaryRates.Subordinate / unitRate;
        }
        public decimal GetSalaryDirect(List<Employee> subList, DateTime salaryDate)
        {
            return subList.Sum(emp => Program.SalaryCache.ContainsKey(emp.ID) ? 
                                      Program.SalaryCache.GetValue(emp.ID) : emp.GetSalary(salaryDate));
        }
    }
    public class SalaryPersonalCalculator : SalaryCalculator
    {
        public SalaryPersonalCalculator(SalaryRates salaryRates) : base(salaryRates) { }
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
    public interface ISalarySubCalculator
    {
        decimal GetSalary(List<Employee> subList, DateTime salaryDate);
    }
    class SalarySubEmployee : SalaryCalculator, ISalarySubCalculator
    {
        public SalarySubEmployee(SalaryRates salaryRates) : base(salaryRates) { }
        public decimal GetSalary(List<Employee> subList, DateTime salaryDate)
        {
            return 0;
        }
    }
    class SalarySubManager : SalaryCalculator, ISalarySubCalculator
    {
        public SalarySubManager(SalaryRates salaryRates) : base(salaryRates) { }
        public decimal GetSalary(List<Employee> subList, DateTime salaryDate)
        {
            return SubordinateRate * GetSalaryDirect(subList, salaryDate);
        }
    }
    class SalarySubSalesman : SalaryCalculator, ISalarySubCalculator
    {
        public SalarySubSalesman(SalaryRates salaryRates) : base(salaryRates) { }
        public decimal GetSalary(List<Employee> subList, DateTime salaryDate)
        {
            return SubordinateRate * (GetSalaryDirect(subList, salaryDate) + GetSalaryIndirect(subList, salaryDate));
        }
        private decimal GetSalaryIndirect(List<Employee> subList, DateTime salaryDate)
        {
            return subList.Sum(emp => GetSalaryDirect(emp.SubordinateList, salaryDate));
        }
    }

    public struct SalaryRates
    {
        public int Experience;
        public int Limit;
        public decimal Subordinate;
    }
    public class SalaryFactory
    {
        public struct SalaryCalculators
        {
            public SalaryPersonalCalculator SalaryPersonal;
            public ISalarySubCalculator SalarySub;
        }
        private Dictionary<Group, SalaryCalculators> SalaryDictionary { get; set; }
        public SalaryFactory()
        {
            var salaryRateEmployee = new SalaryRates { Experience = 3, Limit = 30 };
            var salaryRateManager = new SalaryRates { Experience = 5, Limit = 40, Subordinate = 0.5m };
            var salaryRateSalesman = new SalaryRates { Experience = 1, Limit = 35, Subordinate = 0.3m };
            SalaryDictionary = new Dictionary<Group, SalaryCalculators> {
                {Group.Employee, new SalaryCalculators { SalarySub = new SalarySubEmployee(salaryRateEmployee),
                                                SalaryPersonal = new SalaryPersonalCalculator(salaryRateEmployee) } },
                {Group.Manager, new SalaryCalculators { SalarySub = new SalarySubManager(salaryRateManager),
                                               SalaryPersonal = new SalaryPersonalCalculator(salaryRateManager) }  },
                {Group.Salesman, new SalaryCalculators { SalarySub = new SalarySubSalesman(salaryRateSalesman),
                                                SalaryPersonal = new SalaryPersonalCalculator(salaryRateSalesman) }  }
            };
        }
        public SalaryCalculators GetSalaryCalculator(Group group)
        {
            return SalaryDictionary[group];
        }
    }
}

