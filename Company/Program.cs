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
            ISubordinateCalculator subordinateEmployee = salaryFactory.GetSubordinateCalculator(Groups.Employee);
            ISubordinateCalculator subordinateManager = salaryFactory.GetSubordinateCalculator(Groups.Manager);
            ISubordinateCalculator subordinateSalesman = salaryFactory.GetSubordinateCalculator(Groups.Salesman);
            List<Employee> employeeList = new List<Employee> {
    new Employee(1, "Смит", DateTime.Parse("5/5/5"), Groups.Employee, 15, new List<int> { }, salaryEmployee, subordinateEmployee),
    new Employee(2, "Гейтс", DateTime.Parse("7/7/7"), Groups.Manager, 30, new List<int> {1, 6}, salaryManager, subordinateManager),
    new Employee(3, "Трамп", DateTime.Parse("11/12/13"), Groups.Salesman, 35, new List<int> {2, 4, 8}, salarySalesman, subordinateSalesman),
    new Employee(4, "Паркер", DateTime.Parse("8/4/2"), Groups.Employee, 20, new List<int> { }, salaryEmployee, subordinateEmployee),
    new Employee(5, "Морган", DateTime.Parse("15/10/15"), Groups.Manager, 40, new List<int> {3}, salaryManager, subordinateManager),
    new Employee(6, "Хьюз", DateTime.Parse("7/5/75"), Groups.Salesman, 25, new List<int> {7}, salarySalesman, subordinateSalesman),
    new Employee(7, "МакФлай", DateTime.Parse("31/1/13"), Groups.Employee, 10, new List<int> { }, salaryEmployee, subordinateEmployee),
    new Employee(8, "Уиллис", DateTime.Parse("1/1/1"), Groups.Manager, 45, new List<int> { }, salaryManager, subordinateManager)
            };
            foreach (Employee employee in employeeList)
            {
                employee.CalculateSalary(employeeList);
                employee.SalaryWrite();
            }
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
        public DateTime DateHire { get; set; }
        private Groups Group { get; set; }
        public int SalaryBase { get; set; }
        public List<int> SubordinateDirectListID { get; set; }
        public SalaryCalculator SalaryCalculator { get; set; }
        public ISubordinateCalculator SubordinateCalculator { get; set; }
        public Employee(int id, string name, DateTime dateHire, Groups group, int salaryBase, List<int> subordinateDirectListID,
            SalaryCalculator salaryCalculator, ISubordinateCalculator subordinateCalculator)
        {
            ID = id;
            Name = name;
            DateHire = dateHire;
            Group = group;
            SalaryBase = salaryBase;
            SubordinateDirectListID = subordinateDirectListID;
            SalaryCalculator = salaryCalculator;
            SubordinateCalculator = subordinateCalculator;
            Experience = GetExperience(dateHire);
        }
        public decimal GetSubordinateSalary(List<Employee> employeeList)
        {
            return GetSubordinate(employeeList).Sum(emp => emp.GetSalary(employeeList));
        }
        public List<Employee> GetSubordinate(List<Employee> employeeList)
        {
            return SubordinateCalculator.GetSubordinate(this, employeeList);
        }

        public int Experience { get; set; }
        public int GetExperience(DateTime dateHire)
        {
            var today = DateTime.Today;
            var exp = today.Year - dateHire.Year;
            if (dateHire.Date > today.AddYears(-exp)) exp--;
            return exp;
        }
        public decimal SalaryCalculated { get; set; }
        public void CalculateSalary(List<Employee> employeeList)
        {
            SalaryCalculated = SalaryCalculator.GetSalaryTotal(this, employeeList);
        }
        public decimal GetSalary(List<Employee> employeeList)
        {
            CalculateSalary(employeeList);
            return SalaryCalculated;
        }
        public void SalaryWrite()
        {
            Console.WriteLine(ID + " " + Name + " " + Group + " " + DateHire.ToString("dd MMMM yyyy") + " зп = " + Round(SalaryCalculated));
        }
    }

    public interface ISubordinateCalculator
    {
        List<Employee> GetSubordinate(Employee employee, List<Employee> employeeList);
    }
    abstract class SubordinateCalculator
    {
        public List<Employee> GetSubordinateDirect(Employee employee, List<Employee> employeeList)
        {
            return employeeList.Where(emp => employee.SubordinateDirectListID.Contains(emp.ID)).ToList();
        }
    }
    class SubordinateNonSalesman : SubordinateCalculator, ISubordinateCalculator
    {
        public List<Employee> GetSubordinate(Employee employee, List<Employee> employeeList)
        {
            return GetSubordinateDirect(employee, employeeList);
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
        public List<Employee> GetSubordinate(Employee employee, List<Employee> employeeList)
        {
            List<Employee> subordinateList = GetSubordinateDirect(employee, employeeList);
            subordinateList.AddRange(GetSubordinateIndirect(employeeList, subordinateList));
            return subordinateList;
        }
    }

    public class SalaryCalculator
    {
        public int ExperienceRate { get; set; }
        public int LimitRate { get; set; }
        public decimal SubordinateRate { get; set; }
        public SalaryCalculator(SalaryRates salaryRates)
        {
            ExperienceRate = salaryRates.Experience;
            LimitRate = salaryRates.Limit;
            SubordinateRate = salaryRates.Subordinate;
        }
        private int GetSalaryBase(int salaryBase)
        {
            return salaryBase;
        }
        private decimal GetExperienceRate(int experience)
        {
            return (decimal)Min(ExperienceRate * experience, LimitRate) / 100 + 1;
        }
        private decimal GetSalaryPersonal(Employee employee)
        {
            return GetSalaryBase(employee.SalaryBase) * GetExperienceRate(employee.Experience);
        }
        private decimal GetSalarySubordinate(Employee employee, List<Employee> employeeList)
        {
            return SubordinateRate * employee.GetSubordinateSalary(employeeList) / 100;
        }
        public decimal GetSalaryTotal(Employee employee, List<Employee> employeeList)
        {
            return GetSalaryPersonal(employee) + GetSalarySubordinate(employee, employeeList);
        }
    }

    public class SalaryFactory
    {
        private Dictionary<Groups, SalaryCalculator> SalaryDictionary { get; set; }
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
        public ISubordinateCalculator GetSubordinateCalculator(Groups group)
        {
            return SubordinateDictionary[group];
        }
    }

}

