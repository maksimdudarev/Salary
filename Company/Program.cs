using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace Company
{
    class Program
    {
        static void Main(string[] args)
        {
            SalaryFactory salaryFactory = new SalaryFactory();
            ISalaryCalculator salaryEmployee = salaryFactory.GetSalaryCalculator(Groups.Employee);
            ISalaryCalculator salaryManager = salaryFactory.GetSalaryCalculator(Groups.Manager);
            ISalaryCalculator salarySalesman = salaryFactory.GetSalaryCalculator(Groups.Salesman);
            List<Employee> employeeList = new List<Employee> {
                new Employee(1, "Смит", DateTime.Parse("5/5/5"), Groups.Employee, 15, new List<int> { }, salaryEmployee),
                new Employee(2, "Гейтс", DateTime.Parse("7/7/7"), Groups.Manager, 30, new List<int> {1, 6}, salaryManager),
                new Employee(3, "Трамп", DateTime.Parse("11/12/13"), Groups.Salesman, 35, new List<int> {2, 4, 8}, salarySalesman),
                new Employee(4, "Паркер", DateTime.Parse("8/4/2"), Groups.Employee, 20, new List<int> { }, salaryEmployee),
                new Employee(5, "Морган", DateTime.Parse("15/10/15"), Groups.Manager, 40, new List<int> {3}, salaryManager),
                new Employee(6, "Хьюз", DateTime.Parse("7/5/75"), Groups.Salesman, 25, new List<int> {7}, salarySalesman),
                new Employee(7, "МакФлай", DateTime.Parse("31/1/13"), Groups.Employee, 10, new List<int> { }, salaryEmployee),
                new Employee(8, "Уиллис", DateTime.Parse("1/1/1"), Groups.Manager, 45, new List<int> { }, salaryManager)
            };
            DateTime salaryDate;
            salaryDate = DateTime.Today;
            //salaryDate = DateTime.Parse("17/1/7");
            foreach (Employee employee in employeeList)
            {
                employee.SalaryWrite(employee.GetSalary(employeeList, salaryDate));
            }
            Console.WriteLine("Итого = " + Round(employeeList.Sum(emp => emp.GetSalary(employeeList, salaryDate))));
            Console.Read();
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
        public int ID { get; }
        private string Name { get; set; }
        public DateTime HireDate { get; set; }
        private Groups Group { get; set; }
        public int SalaryBase { get; set; }
        public List<int> SubordinateDirectID { get; set; }
        public ISalaryCalculator SalaryCalculator { get; set; }
        public Employee(int id, string name, DateTime hireDate, Groups group, int salaryBase, List<int> subordinateDirectID,
            ISalaryCalculator salaryCalculator)
        {
            ID = id;
            Name = name;
            HireDate = hireDate;
            Group = group;
            SalaryBase = salaryBase;
            SubordinateDirectID = subordinateDirectID;
            SalaryCalculator = salaryCalculator;
        }
        public decimal GetSalary(List<Employee> employeeList, DateTime? salaryDateOptional = null)
        {
            return SalaryCalculator.GetSalary(this, employeeList, salaryDateOptional ?? DateTime.Today);
        }
        public void SalaryWrite(decimal salaryCalculated)
        {
            Console.WriteLine(ID + " " + Name + " " + Group + " " + HireDate.ToString("dd MMMM yyyy") + " зп = " + Round(salaryCalculated));
        }
    }

    public interface ISalaryCalculator { decimal GetSalary(Employee employee, List<Employee> employeeList, DateTime salaryDate); }
    public class SalaryCalculator
    {
        private decimal ExperienceRate { get; set; }
        private decimal LimitRate { get; set; }
        public decimal SubordinateRate { get; set; }
        public SalaryCalculator(SalaryRates salaryRates)
        {
            decimal unitRate = 100;
            ExperienceRate = salaryRates.Experience / unitRate;
            LimitRate = salaryRates.Limit / unitRate;
            SubordinateRate = salaryRates.Subordinate / unitRate;
        }
        private int GetExperience(DateTime salaryDate, DateTime hireDate)
        {
            var exp = salaryDate.Year - hireDate.Year;
            if (hireDate.Date > salaryDate.AddYears(-exp)) exp--;
            return exp;
        }
        public decimal GetSalaryPersonal(Employee employee, DateTime salaryDate)
        {
            return (Min(ExperienceRate * GetExperience(salaryDate, employee.HireDate), LimitRate) + 1) * employee.SalaryBase;
        }
        //-----
        public decimal GetSalaryDirect(List<Employee> employeeList, List<int> subordinateDirectID, DateTime salaryDate)
        {
            return GetSubordinate(employeeList, subordinateDirectID).Sum(emp => emp.GetSalary(employeeList, salaryDate));
        }
        public List<Employee> GetSubordinate(List<Employee> employeeList, List<int> subordinateDirectID)
        {
            return employeeList.Where(emp => subordinateDirectID.Contains(emp.ID)).ToList();
        }
    }
    class SalaryNonSalesman : SalaryCalculator, ISalaryCalculator
    {
        public SalaryNonSalesman(SalaryRates salaryRates) : base(salaryRates) { }
        public decimal GetSalary(Employee employee, List<Employee> employeeList, DateTime salaryDate)
        {
            return GetSalaryPersonal(employee, salaryDate) + 
                SubordinateRate * GetSalaryDirect(employeeList, employee.SubordinateDirectID, salaryDate);
        }
    }
    class SalarySalesman : SalaryCalculator, ISalaryCalculator
    {
        public SalarySalesman(SalaryRates salaryRates) : base(salaryRates) { }
        private decimal GetSalaryIndirect(List<Employee> employeeList, List<int> subordinateDirectID, DateTime salaryDate)
        {
            return GetSubordinate(employeeList, subordinateDirectID).
                Sum(emp => GetSalaryDirect(employeeList, emp.SubordinateDirectID, salaryDate));
        }
        public decimal GetSalary(Employee employee, List<Employee> employeeList, DateTime salaryDate)
        {
            return GetSalaryPersonal(employee, salaryDate) + SubordinateRate * 
                (GetSalaryDirect(employeeList, employee.SubordinateDirectID, salaryDate) + 
                GetSalaryIndirect(employeeList, employee.SubordinateDirectID, salaryDate));
        }
    }

    public class SalaryFactory
    {
        private Dictionary<Groups, ISalaryCalculator> SalaryDictionary { get; set; }
        public SalaryFactory()
        {
            SalaryRates salaryRateEmployee = new SalaryRates { Experience = 3, Limit = 30 };
            SalaryRates salaryRateManager = new SalaryRates { Experience = 5, Limit = 40, Subordinate = 0.5m };
            SalaryRates salaryRateSalesman = new SalaryRates { Experience = 1, Limit = 35, Subordinate = 0.3m };
            SalaryDictionary = new Dictionary<Groups, ISalaryCalculator> {
                {Groups.Employee, new SalaryNonSalesman(salaryRateEmployee) },
                {Groups.Manager, new SalaryNonSalesman(salaryRateManager) },
                {Groups.Salesman, new SalarySalesman(salaryRateSalesman) }
            };
        }
        public ISalaryCalculator GetSalaryCalculator(Groups group)
        {
            return SalaryDictionary[group];
        }
    }

}

