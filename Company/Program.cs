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
            List<Employee> employeeList = new List<Employee> {
                new Employee(1, "Смит", DateTime.Parse("5/5/5"), Groups.Employee, 15, new List<int> {}),
                new Employee(2, "Гейтс", DateTime.Parse("7/7/7"), Groups.Manager, 30, new List<int> {1, 6}),
                new Employee(3, "Трамп", DateTime.Parse("11/12/13"), Groups.Salesman, 35, new List<int> {2, 4}),
                new Employee(4, "Паркер", DateTime.Parse("8/4/2"), Groups.Employee, 20, new List<int> {}),
                new Employee(5, "Морган", DateTime.Parse("15/10/15"), Groups.Manager, 40, new List<int> {3}),
                new Employee(6, "Хьюз", DateTime.Parse("3/9/3"), Groups.Salesman, 25, new List<int> {7}),
                new Employee(7, "МакФлай", DateTime.Parse("31/1/13"), Groups.Employee, 10, new List<int> {})
            };
            foreach (Employee employee in employeeList)
            {
                employee.SalaryCalculate(employeeList);
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
        private DateTime HireDate { get; set; }
        private Groups Group { get; set; }
        public int SalaryBase { get; set; }
        public List<int> SubordinateList { get; set; }
        public Employee(int id, string name, DateTime hireDate, Groups group, int salaryBase, List<int> subordinateList)
        {
            ID = id;
            Name = name;
            HireDate = hireDate;
            Group = group;
            SalaryBase = salaryBase;
            SubordinateList = subordinateList;
        }
        private int SalaryCalculated { get; set; }
        public void SalaryCalculate(List<Employee> employeeList)
        {
            SalaryCalculated = new SalaryFactory().GetSalaryCalculator(Group).GetSalary(employeeList, ID);
        }
        public void SalaryWrite()
        {
            Console.WriteLine(Name + " " + Group + " " + HireDate.ToString("dd MMMM yyyy") + " зп = " + SalaryCalculated);
        }
    }

    public interface ISalaryCalculator
    {
        int GetSalary(List<Employee> employeeList, int employeeID);
        int GetSalarySubordinate();
    }
    abstract class Salary
    {
        public int GetSalaryBase(List<Employee> employeeList, int subordinateID)
        {
            return employeeList.Find(s => s.ID == subordinateID).SalaryBase;
        }
        public List<int> GetSubordinateList(List<Employee> employeeList, int employeeID)
        {
            return employeeList.Find(s => s.ID == employeeID).SubordinateList;
        }
    }
    class SalaryEmployee : Salary, ISalaryCalculator
    {
        public int GetSalary(List<Employee> employeeList, int employeeID)
        {
            return GetSalaryBase(employeeList, employeeID);
        }
        public int GetSalarySubordinate()
        {
            return 0;
        }
    }
    class SalaryManager : Salary, ISalaryCalculator
    {
        public int GetSalary(List<Employee> employeeList, int employeeID)
        {
            int sum = 0;
            foreach (int subordinateID in GetSubordinateList(employeeList, employeeID))
                sum += GetSalary(employeeList, subordinateID) + GetSalaryBase(employeeList, subordinateID);
            return sum;
        }
        public int GetSalarySubordinate()
        {
            return 0;
        }
    }
    class SalarySalesman : Salary, ISalaryCalculator
    {
        public int GetSalary(List<Employee> employeeList, int employeeID)
        {
            return 0;
        }
        public int GetSalarySubordinate()
        {
            return 0;
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

