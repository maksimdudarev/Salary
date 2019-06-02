using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                new Employee(3, "Трамп", DateTime.Parse("11/12/13"), Groups.Salesman, 35, new List<int> {2, 4}, salarySalesman),
                new Employee(4, "Паркер", DateTime.Parse("8/4/2"), Groups.Employee, 20, new List<int> { }, salaryEmployee),
                new Employee(5, "Морган", DateTime.Parse("15/10/15"), Groups.Manager, 40, new List<int> {3}, salaryManager),
                new Employee(6, "Хьюз", DateTime.Parse("7/5/75"), Groups.Salesman, 25, new List<int> {7}, salarySalesman),
                new Employee(7, "МакФлай", DateTime.Parse("31/1/13"), Groups.Employee, 10, new List<int> { }, salaryEmployee)
            };
            for (int i = 0; i < employeeList.Count; i++)
            {
                employeeList[i].CalculateSubordinateAllList(employeeList);
            }
            foreach (Employee employee in employeeList)
            {
                employee.CalculateSalary();
                employee.SalaryWrite();
            }
            Console.Read();
        }
    }

    public enum Groups { Employee, Manager, Salesman }
    public enum ExperienceRates { Employee = 3, Manager = 5, Salesman = 1 }
    public enum LimitRates { Employee = 30, Manager = 40, Salesman = 35 }

    public class Employee
    {
        public int ID { get; }
        private string Name { get; set; }
        public DateTime DateHire { get; set; }
        private Groups Group { get; set; }
        public int SalaryBase { get; set; }
        public List<int> SubordinateDirectListID { get; set; }
        public List<Employee> SubordinateDirectList { get; set; }
        public List<Employee> SubordinateAllList { get; set; }
        public ISalaryCalculator SalaryCalculator { get; set; }
        public Employee(int id, string name, DateTime dateHire, Groups group, int salaryBase, List<int> subordinateDirectListID,
            ISalaryCalculator salaryCalculator)
        {
            ID = id;
            Name = name;
            DateHire = dateHire;
            Group = group;
            SalaryBase = salaryBase;
            SubordinateDirectListID = subordinateDirectListID;
            SalaryCalculator = salaryCalculator;
            Experience = GetExperience(dateHire);
        }
        public void CalculateSubordinateDirectList(List<Employee> employeeList)
        {
            SubordinateDirectList = employeeList.Where(emp => SubordinateDirectListID.Contains(emp.ID)).ToList();
        }
        public void CalculateSubordinateAllList(List<Employee> employeeList)
        {
            SubordinateAllList = new List<Employee>();
            CalculateSubordinateDirectList(employeeList);
            if (SubordinateDirectList.Count > 0) SubordinateAllList.AddRange(SubordinateDirectList);
            foreach (var emp in SubordinateDirectList)
            {
                emp.CalculateSubordinateAllList(employeeList);
                if (emp.SubordinateAllList.Count > 0) SubordinateAllList.AddRange(emp.SubordinateAllList);
            }
        }
        public int Experience { get; set; }
        public int GetExperience(DateTime dateHire)
        {
            var today = DateTime.Today;
            var exp = today.Year - dateHire.Year;
            if (dateHire.Date > today.AddYears(-exp)) exp--;
            return exp;
        }
        public int SalaryCalculated { get; set; }
        public void CalculateSalary()
        {
            SalaryCalculated = SalaryCalculator.GetSalary(SubordinateDirectList, this);
        }
        public int GetSalary()
        {
            CalculateSalary();
            return SalaryCalculated;
        }
        public void SalaryWrite()
        {
            Console.WriteLine(ID + " " + Name + " " + Group + " " + DateHire.ToString("dd MMMM yyyy") + " зп = " + SalaryCalculated);
        }
    }

    public interface ISalaryCalculator
    {
        int GetSalary(List<Employee> subordinateDirectList, Employee employee);
    }
    abstract class SalaryCalculator
    {
        public int ExperienceRate { get; set; }
        public int LimitRate { get; set; }
        public SalaryCalculator(int experienceRate, int limitRate)
        {
            ExperienceRate = experienceRate;
            LimitRate = limitRate;
        }
        private int GetSalaryBase(int salaryBase)
        {
            return salaryBase;
        }
        private decimal GetExperienceRate(int experience)
        {
            return (decimal)Math.Min(ExperienceRate * experience, LimitRate) / 100 + 1;
        }
        public int GetSalaryPersonal(Employee employee)
        {
            return (int)(GetSalaryBase(employee.SalaryBase) * GetExperienceRate(employee.Experience));
        }
    }
    class SalaryEmployee : SalaryCalculator, ISalaryCalculator
    {
        public SalaryEmployee(int experienceRate, int limitRate) : base(experienceRate, limitRate) { }
        public int GetSalary(List<Employee> subordinateDirectList, Employee employee)
        {
            return GetSalaryPersonal(employee);
        }
    }
    class SalaryManager : SalaryCalculator, ISalaryCalculator
    {
        public SalaryManager(int experienceRate, int limitRate) : base(experienceRate, limitRate) { }
        public int GetSalary(List<Employee> subordinateDirectList, Employee employee)
        {
            return GetSalaryPersonal(employee) + subordinateDirectList.Sum(s => s.GetSalary());
        }
    }
    class SalarySalesman : SalaryCalculator, ISalaryCalculator
    {
        public SalarySalesman(int experienceRate, int limitRate) : base(experienceRate, limitRate) { }
        public int GetSalary(List<Employee> subordinateDirectList, Employee employee)
        {
            return GetSalaryPersonal(employee) + subordinateDirectList.Sum(s => s.GetSalary());
        }
    }

    public class SalaryFactory
    {
        private Dictionary<Groups, ISalaryCalculator> SalaryDictionary { get; set; }
        public SalaryFactory()
        {
            SalaryDictionary = new Dictionary<Groups, ISalaryCalculator> {
                {Groups.Employee, new SalaryEmployee((int)ExperienceRates.Employee, (int)LimitRates.Employee) },
                {Groups.Manager, new SalaryManager((int)ExperienceRates.Manager, (int)LimitRates.Manager) },
                {Groups.Salesman, new SalarySalesman((int)ExperienceRates.Salesman, (int)LimitRates.Salesman) }
            };
        }
        public ISalaryCalculator GetSalaryCalculator(Groups group)
        {
            return SalaryDictionary[group];
        }
    }

}

