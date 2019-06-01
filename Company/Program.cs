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
                new Employee(1, "Смит", DateTime.Parse("5/5/5"), Groups.Employee, 15, new List<int> {}, salaryEmployee),
                new Employee(2, "Гейтс", DateTime.Parse("7/7/7"), Groups.Manager, 30, new List<int> {1, 6}, salaryManager),
                new Employee(3, "Трамп", DateTime.Parse("11/12/13"), Groups.Salesman, 35, new List<int> {2, 4}, salarySalesman),
                new Employee(4, "Паркер", DateTime.Parse("8/4/2"), Groups.Employee, 20, new List<int> {}, salaryEmployee),
                new Employee(5, "Морган", DateTime.Parse("15/10/15"), Groups.Manager, 40, new List<int> {3}, salaryManager),
                new Employee(6, "Хьюз", DateTime.Parse("3/8/3"), Groups.Salesman, 25, new List<int> {7}, salarySalesman),
                new Employee(7, "МакФлай", DateTime.Parse("31/1/13"), Groups.Employee, 10, new List<int> {}, salaryEmployee)
            };
            for (int i = 0; i < employeeList.Count; i++)
            {
                employeeList[i].EmployeeList = employeeList;
            }
            foreach (Employee employee in employeeList)
            {
                employee.CalculateSalary();
                employee.SalaryWrite();
            }
            Console.Read();
        }
    }

    public enum Groups
    {
        Employee, Manager, Salesman
    }

    public class Employee
    {
        public int ID { get; }
        private string Name { get; set; }
        public DateTime DateHire { get; set; }
        private Groups Group { get; set; }
        public int SalaryBase { get; set; }
        public List<int> SubordinateListID { get; set; }
        public ISalaryCalculator SalaryCalculator { get; set; }
        public Employee(int id, string name, DateTime dateHire, Groups group, int salaryBase, List<int> subordinateListID,
            ISalaryCalculator salaryCalculator)
        {
            ID = id;
            Name = name;
            DateHire = dateHire;
            Group = group;
            SalaryBase = salaryBase;
            SubordinateListID = subordinateListID;
            SalaryCalculator = salaryCalculator;
        }
        public List<Employee> EmployeeList { get; set; }
        public List<Employee> GetSubordinateList()
        {
            return EmployeeList.Where(emp => SubordinateListID.Contains(emp.ID)).ToList();
        }
        public int SalaryCalculated { get; set; }
        public void CalculateSalary()
        {
            SalaryCalculated = SalaryCalculator.GetSalary(GetSubordinateList(), this);
        }
        public int GetSalary()
        {
            CalculateSalary();
            return SalaryCalculated;
        }
        public void SalaryWrite()
        {
            Console.WriteLine(Name + " " + Group + " " + DateHire.ToString("dd MMMM yyyy") + " зп = " + SalaryCalculated);
        }
    }

    public interface ISalaryCalculator
    {
        int GetSalary(List<Employee> subordinateList, Employee employee);
    }
    abstract class SalaryCalculator
    {
        public int GetSalaryBase(int salaryBase)
        {
            return salaryBase;
        }
        public int GetSalaryExperience(DateTime dateHire)
        {
            var today = DateTime.Today;
            var exp = today.Year - dateHire.Year;
            if (dateHire.Date > today.AddYears(-exp)) exp--;
            return exp;
        }
    }
    class SalaryEmployee : SalaryCalculator, ISalaryCalculator
    {
        public int GetSalary(List<Employee> subordinateList, Employee employee)
        {
            return GetSalaryBase(employee.SalaryBase);
        }
    }
    class SalaryManager : SalaryCalculator, ISalaryCalculator
    {
        public int GetSalary(List<Employee> subordinateList, Employee employee)
        {
            return GetSalaryBase(employee.SalaryBase) + GetSalaryExperience(employee.DateHire) + subordinateList.Sum(s => s.GetSalary());
        }
    }
    class SalarySalesman : SalaryCalculator, ISalaryCalculator
    {
        public int GetSalary(List<Employee> subordinateList, Employee employee)
        {
            return GetSalaryBase(employee.SalaryBase) + subordinateList.Sum(s => s.GetSalary());
        }
    }

    public class SalaryFactory
    {
        private Dictionary<Groups, ISalaryCalculator> SalaryDictionary { get; set; }
        public SalaryFactory()
        {
            SalaryDictionary = new Dictionary<Groups, ISalaryCalculator> {
                {Groups.Employee, new SalaryEmployee() },
                {Groups.Manager, new SalaryManager() },
                {Groups.Salesman, new SalarySalesman() }
            };
        }
        public ISalaryCalculator GetSalaryCalculator(Groups group)
        {
            return SalaryDictionary[group];
        }
    }

}

