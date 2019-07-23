using System.Collections.Generic;

namespace MD.Salary.ConsoleApp.Models
{
    public enum Group { Employee, Manager, Salesman }
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
                {Group.Employee, new Calculators {Sub = new CalculatorSubEmployee(0), Personal = new CalculatorPersonal(3, 30) } },
                {Group.Manager, new Calculators {Sub = new CalculatorSubManager(0.5m), Personal = new CalculatorPersonal(5, 40) } },
                {Group.Salesman, new Calculators {Sub = new CalculatorSubSalesman(0.3m), Personal = new CalculatorPersonal(1, 35) } }
            };
        }
        public Calculators GetCalculator(Group group)
        {
            return SalaryDictionary[group];
        }
    }
}
