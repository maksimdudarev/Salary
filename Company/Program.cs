using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company
{
    class Program
    {
        /*
Тестовое задание
Для отдела HR одной компании нужно написать приложение расчета заработной платы.
В компании работают сотрудники, характеризующиеся именем, датой поступления на работу, группой и базовой ставкой заработной платы.
Есть 3 группы сотрудников - Employee, Manager и Salesman. У каждого сотрудника может быть начальник. У каждого сотрудника кроме Employee могут быть подчинённые.
Зарплата сотрудника рассчитывается следующим образом:
⦁	Employee - это базовая ставка плюс 3% за каждый год работы, но не больше 30% суммарной надбавки. 
⦁	Manager - это базовая ставка плюс 5% за каждый год работы, но не больше 40% суммарной надбавки за стаж работы. Плюс 0,5% зарплаты всех подчинённых первого уровня.
⦁	Salesman - это базовая ставка плюс 1% за каждый год работы в компании, но не больше 35% суммарной надбавки за стаж работы. Плюс 0,3% зарплаты всех подчинённых всех уровней.
⦁	У сотрудников (кроме Employee) может быть любое количество подчинённых любой группы.
Требуется: составить структуру классов, описывающих данную модель, а также реализовать алгоритм расчета зарплаты каждого сотрудника на произвольный момент времени, а также подсчёт суммарной зарплаты всех сотрудников фирмы в целом.
Замечание: при реализации тестового задания необходимо предположить, что вы разрабатываете не просто прототип, а систему enterprise уровня, соответственно важнее продемонстрировать архитектурно более красивое решение, даже в ущерб быстродействию. Не обязательно реализовывать всю архитектуру в полном объёме, не реализованные или упрощённые моменты нужно прокомментировать.
Решение: нужно сделать на C# с использованием sqlite, применяя любые библиотеки.
Код передать в виде репозитория git на github или аналоге.

Дополнительные плюсы:
⦁	Написан краткий обзор решения тестовой задачи, описана архитектура, ее плюсы и минусы (что можно улучшить, поменять или еще какие-то соображения для использования решения в реальных целях).
⦁	Код покрыт тестами.
⦁	Программа имеет графический интерфейс.
⦁	Будет возможность просмотреть для выбранного сотрудника список его подчинённых.
⦁	Будет возможность добавлять новых сотрудников разных видов
⦁	Будет возможность разграничения прав, каждый сотрудник будет иметь свой логин/пароль, имея возможность просматривать только свою зарплату, и зарплату своих подчинённых. Также должен быть супер-пользователь, который имеет доступ ко всем.         
             */
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
        public List<Employee> GetSubordinateDirectList(List<Employee> employeeList)
        {
            return employeeList.Where(emp => SubordinateDirectListID.Contains(emp.ID)).ToList();
        }
        public List<Employee> GetSubordinateAllList(List<Employee> employeeList)
        {
            List<Employee> subordinateAllList = GetSubordinateDirectList(employeeList);
            if (Group == Groups.Salesman)
            {
                List<Employee> subordinateSubList = new List<Employee>();
                foreach (var emp in subordinateAllList)
                {
                    subordinateSubList.AddRange(emp.GetSubordinateAllList(employeeList));
                }
                subordinateAllList.AddRange(subordinateSubList);
            }
            return subordinateAllList;
        }
        public decimal GetSubordinateAllSalary(List<Employee> employeeList)
        {
            return GetSubordinateAllList(employeeList).Sum(s => s.GetSalary(employeeList));
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
            SalaryCalculated = SalaryCalculator.GetSalary(this, employeeList);
        }
        public decimal GetSalary(List<Employee> employeeList)
        {
            CalculateSalary(employeeList);
            return SalaryCalculated;
        }
        public void SalaryWrite()
        {
            Console.WriteLine(ID + " " + Name + " " + Group + " " + DateHire.ToString("dd MMMM yyyy") + " зп = " + SalaryCalculated);
        }
    }

    public interface ISalaryCalculator
    {
        decimal GetSalary(Employee employee, List<Employee> employeeList);
    }
    abstract class SalaryCalculator
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
            return (decimal)Math.Min(ExperienceRate * experience, LimitRate) / 100 + 1;
        }
        public decimal GetSalaryPersonal(Employee employee)
        {
            return GetSalaryBase(employee.SalaryBase) * GetExperienceRate(employee.Experience);
        }
        public decimal GetSalarySubordinate(Employee employee, List<Employee> employeeList)
        {
            return employee.GetSubordinateAllSalary(employeeList) / 100;
        }
    }
    class SalaryEmployee : SalaryCalculator, ISalaryCalculator
    {
        public SalaryEmployee(SalaryRates salaryRates) : base(salaryRates) { }
        public decimal GetSalary(Employee employee, List<Employee> employeeList)
        {
            return GetSalaryPersonal(employee);
        }
    }
    class SalaryManager : SalaryCalculator, ISalaryCalculator
    {
        public SalaryManager(SalaryRates salaryRates) : base(salaryRates) { }
        public decimal GetSalary(Employee employee, List<Employee> employeeList)
        {
            return GetSalaryPersonal(employee) + GetSalarySubordinate(employee, employeeList);
        }
    }
    class SalarySalesman : SalaryCalculator, ISalaryCalculator
    {
        public SalarySalesman(SalaryRates salaryRates) : base(salaryRates) { }
        public decimal GetSalary(Employee employee, List<Employee> employeeList)
        {
            return GetSalaryPersonal(employee) + GetSalarySubordinate(employee, employeeList);
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
                {Groups.Employee, new SalaryEmployee(salaryRateEmployee) },
                {Groups.Manager, new SalaryManager(salaryRateManager) },
                {Groups.Salesman, new SalarySalesman(salaryRateSalesman) }
            };
        }
        public ISalaryCalculator GetSalaryCalculator(Groups group)
        {
            return SalaryDictionary[group];
        }
    }

}

