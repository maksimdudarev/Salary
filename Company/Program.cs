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
            SalaryCalculator salaryEmployee = salaryFactory.GetSalaryCalculator(Groups.Employee);
            SalaryCalculator salaryManager = salaryFactory.GetSalaryCalculator(Groups.Manager);
            SalaryCalculator salarySalesman = salaryFactory.GetSalaryCalculator(Groups.Salesman);
            ISalaryCalculator2 salaryEmployee2 = salaryFactory.GetSalaryCalculator2(Groups.Employee);
            ISalaryCalculator2 salaryManager2 = salaryFactory.GetSalaryCalculator2(Groups.Manager);
            ISalaryCalculator2 salarySalesman2 = salaryFactory.GetSalaryCalculator2(Groups.Salesman);
            ISubordinateCalculator subordinateEmployee = salaryFactory.GetSubordinateCalculator(Groups.Employee);
            ISubordinateCalculator subordinateManager = salaryFactory.GetSubordinateCalculator(Groups.Manager);
            ISubordinateCalculator subordinateSalesman = salaryFactory.GetSubordinateCalculator(Groups.Salesman);
            List<Employee> employeeList = new List<Employee> {
    new Employee(1, "Смит", DateTime.Parse("5/5/5"), Groups.Employee, 15, new List<int> { }, salaryEmployee, subordinateEmployee, salaryEmployee2),
    new Employee(2, "Гейтс", DateTime.Parse("7/7/7"), Groups.Manager, 30, new List<int> {1, 6}, salaryManager, subordinateManager, salaryManager2),
    new Employee(3, "Трамп", DateTime.Parse("11/12/13"), Groups.Salesman, 35, new List<int> {2, 4, 8}, salarySalesman, subordinateSalesman, salarySalesman2),
    new Employee(4, "Паркер", DateTime.Parse("8/4/2"), Groups.Employee, 20, new List<int> { }, salaryEmployee, subordinateEmployee, salaryEmployee2),
    new Employee(5, "Морган", DateTime.Parse("15/10/15"), Groups.Manager, 40, new List<int> {3}, salaryManager, subordinateManager, salaryManager2),
    new Employee(6, "Хьюз", DateTime.Parse("7/5/75"), Groups.Salesman, 25, new List<int> {7}, salarySalesman, subordinateSalesman, salarySalesman2),
    new Employee(7, "МакФлай", DateTime.Parse("31/1/13"), Groups.Employee, 10, new List<int> { }, salaryEmployee, subordinateEmployee, salaryEmployee2),
    new Employee(8, "Уиллис", DateTime.Parse("1/1/1"), Groups.Manager, 45, new List<int> { }, salaryManager, subordinateManager, salaryManager2)
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
        public SalaryCalculator SalaryCalculator { get; set; }
        public ISalaryCalculator2 SalaryCalculator2 { get; set; }
        public ISubordinateCalculator SubordinateCalculator { get; set; }
        public Employee(int id, string name, DateTime hireDate, Groups group, int salaryBase, List<int> subordinateDirectID,
            SalaryCalculator salaryCalculator, ISubordinateCalculator subordinateCalculator, ISalaryCalculator2 salaryCalculator2)
        {
            ID = id;
            Name = name;
            HireDate = hireDate;
            Group = group;
            SalaryBase = salaryBase;
            SubordinateDirectID = subordinateDirectID;
            SalaryCalculator = salaryCalculator;
            SalaryCalculator2 = salaryCalculator2;
            SubordinateCalculator = subordinateCalculator;
        }
        public decimal GetSalarySubordinate(List<Employee> employeeList, DateTime salaryDate)
        {
            return GetSubordinate(employeeList).Sum(emp => emp.GetSalary(employeeList, salaryDate));
        }
        public List<Employee> GetSubordinate(List<Employee> employeeList)
        {
            return SubordinateCalculator.GetSubordinate(employeeList, SubordinateDirectID);
        }
        public decimal GetSalary(List<Employee> employeeList, DateTime? salaryDateOptional = null)
        {
            var salary2 = SalaryCalculator2.GetSalary(employeeList, SubordinateDirectID);
            return SalaryCalculator.GetSalaryTotal(this, employeeList, salaryDateOptional ?? DateTime.Today);
        }
        public void SalaryWrite(decimal salaryCalculated)
        {
            Console.WriteLine(ID + " " + Name + " " + Group + " " + HireDate.ToString("dd MMMM yyyy") + " зп = " + Round(salaryCalculated));
        }
    }

    public interface ISalaryCalculator2 { decimal GetSalary(List<Employee> employeeList, List<int> subordinateDirectID); }
    class SalaryNonSalesman : SalaryCalculator, ISalaryCalculator2
    {
        public SalaryNonSalesman(SalaryRates salaryRates) : base (salaryRates) {}
        public decimal GetSalary(List<Employee> employeeList, List<int> subordinateDirectID)
        {
            return GetSalaryDirect(employeeList, subordinateDirectID);
        }
    }
    class SalarySalesman : SalaryCalculator, ISalaryCalculator2
    {
        public SalarySalesman(SalaryRates salaryRates) : base(salaryRates) { }
        private decimal GetSalaryIndirect(List<Employee> employeeList, List<int> subordinateDirectID)
        {
            return GetSubordinateDirect(employeeList, subordinateDirectID).
                Sum(emp => GetSalaryDirect(employeeList, emp.SubordinateDirectID));
        }
        public decimal GetSalary(List<Employee> employeeList, List<int> subordinateDirectID)
        {
            return GetSalaryDirect(employeeList, subordinateDirectID) + GetSalaryIndirect(employeeList, subordinateDirectID);
        }
    }

    public interface ISubordinateCalculator { List<Employee> GetSubordinate(List<Employee> employeeList, List<int> subordinateDirectID); }
    abstract class SubordinateCalculator
    {
        public List<Employee> GetSubordinateDirect(List<Employee> employeeList, List<int> subordinateDirectID)
        {
            return employeeList.Where(emp => subordinateDirectID.Contains(emp.ID)).ToList();
        }
    }
    class SubordinateNonSalesman : SubordinateCalculator, ISubordinateCalculator
    {
        public List<Employee> GetSubordinate(List<Employee> employeeList, List<int> subordinateDirectID)
        {
            return GetSubordinateDirect(employeeList, subordinateDirectID);
        }
    }
    class SubordinateSalesman : SubordinateCalculator, ISubordinateCalculator
    {
        private List<Employee> GetSubordinateIndirect(List<Employee> employeeList, List<Employee> managerList)
        {
            List<Employee> subordinateList = new List<Employee>();
            managerList.ForEach(emp => subordinateList.AddRange(emp.GetSubordinate(employeeList)));
            return subordinateList;
        }
        public List<Employee> GetSubordinate(List<Employee> employeeList, List<int> subordinateDirectID)
        {
            List<Employee> subordinateList = GetSubordinateDirect(employeeList, subordinateDirectID);
            subordinateList.AddRange(GetSubordinateIndirect(employeeList, subordinateList));
            return subordinateList;
        }
    }

    public class SalaryCalculator
    {
        private decimal ExperienceRate { get; set; }
        private decimal LimitRate { get; set; }
        private decimal SubordinateRate { get; set; }
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
        private decimal GetSalaryPersonal(Employee employee, DateTime salaryDate)
        {
            return (Min(ExperienceRate * GetExperience(salaryDate, employee.HireDate), LimitRate) + 1) * employee.SalaryBase;
        }
        public decimal GetSalarySubordinate(Employee employee, List<Employee> employeeList, DateTime salaryDate)
        {
            return SubordinateRate * employee.GetSalarySubordinate(employeeList, salaryDate);
        }
        public decimal GetSalaryTotal(Employee employee, List<Employee> employeeList, DateTime salaryDate)
        {
            return GetSalaryPersonal(employee, salaryDate) + GetSalarySubordinate(employee, employeeList, salaryDate);
        }
        //-----
        public decimal GetSalaryDirect(List<Employee> employeeList, List<int> subordinateDirectID)
        {
            return GetSubordinateDirect(employeeList, subordinateDirectID).Sum(emp => emp.GetSalary(employeeList));
        }
        public List<Employee> GetSubordinateDirect(List<Employee> employeeList, List<int> subordinateDirectID)
        {
            return employeeList.Where(emp => subordinateDirectID.Contains(emp.ID)).ToList();
        }
    }

    public class SalaryFactory
    {
        private Dictionary<Groups, SalaryCalculator> SalaryDictionary { get; set; }
        private Dictionary<Groups, ISalaryCalculator2> SalaryDictionary2 { get; set; }
        private Dictionary<Groups, ISubordinateCalculator> SubordinateDictionary { get; set; }
        public SalaryFactory()
        {
            SalaryRates salaryRateEmployee = new SalaryRates { Experience = 3, Limit = 30 };
            SalaryRates salaryRateManager = new SalaryRates { Experience = 5, Limit = 40, Subordinate = 0.5m };
            SalaryRates salaryRateSalesman = new SalaryRates { Experience = 1, Limit = 35, Subordinate = 0.3m };
            SalaryDictionary = new Dictionary<Groups, SalaryCalculator> {
                {Groups.Employee, new SalaryCalculator(salaryRateEmployee) },
                {Groups.Manager, new SalaryCalculator(salaryRateManager) },
                {Groups.Salesman, new SalaryCalculator(salaryRateSalesman) }
            };
            SalaryDictionary2 = new Dictionary<Groups, ISalaryCalculator2> {
                {Groups.Employee, new SalaryNonSalesman(salaryRateEmployee) },
                {Groups.Manager, new SalaryNonSalesman(salaryRateManager) },
                {Groups.Salesman, new SalarySalesman(salaryRateSalesman) }
            };
            SubordinateDictionary = new Dictionary<Groups, ISubordinateCalculator> {
                {Groups.Employee, new SubordinateNonSalesman() },
                {Groups.Manager, new SubordinateNonSalesman() },
                {Groups.Salesman, new SubordinateSalesman() }
            };
        }
        public SalaryCalculator GetSalaryCalculator(Groups group)
        {
            return SalaryDictionary[group];
        }
        public ISalaryCalculator2 GetSalaryCalculator2(Groups group)
        {
            return SalaryDictionary2[group];
        }
        public ISubordinateCalculator GetSubordinateCalculator(Groups group)
        {
            return SubordinateDictionary[group];
        }
    }

}

