using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;
using MD.Salary.Application;

namespace MD.Salary.Model
{
    class SalaryCache
    {
        Dictionary<int, decimal> SalaryCalculated { get; set; }
        public SalaryCache() {SalaryCalculated = new Dictionary<int, decimal> { };}
        public decimal GetSum()
        {
            return SalaryCalculated.Sum(item => item.Value);
        }
        public void Add(int ID, decimal salary)
        {
            if (!Contains(ID)) SalaryCalculated[ID] = salary;
        }
        public decimal Get(int ID)
        {
            return SalaryCalculated[ID];
        }
        public bool Contains(int ID)
        {
            return SalaryCalculated.ContainsKey(ID);
        }
    }

    public enum Groups { Employee, Manager, Salesman }
    public struct SalaryRates
    {
        public int Experience;
        public int Limit;
        public decimal Subordinate;
    }

    public class Employee
    {
        private string Name { get; set; }
        private Groups Group { get; set; }
        public int ID { get; }
        public DateTime HireDate { get; set; }
        public int SalaryBase { get; set; }
        public List<int> SubordinateDirectID { get; set; }
        public List<Employee> SubordinateList { get; set; }
        public ISalarySubCalculator SalarySub { get; set; }
        public SalaryPersonalCalculator SalaryPersonal { get; set; }
        public Employee(int id, string name, DateTime hireDate, Groups group, int salaryBase, List<int> subordinateDirectID)
        {
            ID = id;
            Name = name;
            HireDate = hireDate;
            Group = group;
            SalaryBase = salaryBase;
            SubordinateDirectID = subordinateDirectID;
            var salaryCalculators = new SalaryFactory().GetSalaryCalculator(group);
            SalaryPersonal = salaryCalculators.SalaryPersonal;
            SalarySub = salaryCalculators.SalarySub;
        }
        public void CalculateSubordinate(List<Employee> employeeList)
        {
            SubordinateList = employeeList.Where(emp => SubordinateDirectID.Contains(emp.ID)).ToList();
        }
        public decimal GetSalary(DateTime? salaryDateOptional = null)
        {
            var salaryDate = salaryDateOptional ?? DateTime.Today;
            var salary = SalaryPersonal.GetSalary(SalaryBase, HireDate, salaryDate) + SalarySub.GetSalary(SubordinateList, salaryDate);
            Program.SalaryCache.Add(ID, salary);
            return salary;
        }
        public void SalaryWrite(decimal salaryCalculated)
        {
            Console.WriteLine(ID + " " + Name + " " + Group + " " + HireDate.ToString("dd MMMM yyyy") + " зп = " + Round(salaryCalculated));
        }
    }

    public interface ISalarySubCalculator
    {
        decimal GetSalary(List<Employee> subList, DateTime salaryDate);
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
            return subList.Sum(emp => Program.SalaryCache.Contains(emp.ID) ? 
                                      Program.SalaryCache.Get(emp.ID) : emp.GetSalary(salaryDate));
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
        public decimal GetSalary(int salaryBase, DateTime hireDate, DateTime salaryDate)
        {
            return (Min(ExperienceRate * GetExperience(salaryDate, hireDate), LimitRate) + 1) * salaryBase;
        }
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
        private decimal GetSalaryIndirect(List<Employee> subList, DateTime salaryDate)
        {
            return subList.Sum(emp => GetSalaryDirect(emp.SubordinateList, salaryDate));
        }
        public decimal GetSalary(List<Employee> subList, DateTime salaryDate)
        {
            return SubordinateRate * (GetSalaryDirect(subList, salaryDate) + GetSalaryIndirect(subList, salaryDate));
        }
    }

    public struct SalaryCalculators
    {
        public SalaryPersonalCalculator SalaryPersonal;
        public ISalarySubCalculator SalarySub;
    }
    public class SalaryFactory
    {
        private Dictionary<Groups, SalaryCalculators> SalaryDictionary { get; set; }
        public SalaryFactory()
        {
            var salaryRateEmployee = new SalaryRates { Experience = 3, Limit = 30 };
            var salaryRateManager = new SalaryRates { Experience = 5, Limit = 40, Subordinate = 0.5m };
            var salaryRateSalesman = new SalaryRates { Experience = 1, Limit = 35, Subordinate = 0.3m };
            SalaryDictionary = new Dictionary<Groups, SalaryCalculators> {
                {Groups.Employee, new SalaryCalculators { SalarySub = new SalarySubEmployee(salaryRateEmployee),
                                                SalaryPersonal = new SalaryPersonalCalculator(salaryRateEmployee) } },
                {Groups.Manager, new SalaryCalculators { SalarySub = new SalarySubManager(salaryRateManager),
                                               SalaryPersonal = new SalaryPersonalCalculator(salaryRateManager) }  },
                {Groups.Salesman, new SalaryCalculators { SalarySub = new SalarySubSalesman(salaryRateSalesman),
                                                SalaryPersonal = new SalaryPersonalCalculator(salaryRateSalesman) }  }
            };
        }
        public SalaryCalculators GetSalaryCalculator(Groups group)
        {
            return SalaryDictionary[group];
        }
    }
}

