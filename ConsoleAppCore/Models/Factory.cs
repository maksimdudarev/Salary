using System.Collections.Generic;

namespace MD.Salary.ConsoleApp.Models
{
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
                {Group.Employee, new Calculators {
                    Sub = new CalculatorSubEmployee(
                        Constants.Employee),
                    Personal = new CalculatorPersonal(
                        Constants.Employee) } },
                {Group.Manager, new Calculators {
                    Sub = new CalculatorSubManager(
                        Constants.Manager),
                    Personal = new CalculatorPersonal(
                        Constants.Manager) } },
                {Group.Salesman, new Calculators {
                    Sub = new CalculatorSubSalesman(
                        Constants.Salesman),
                    Personal = new CalculatorPersonal(
                        Constants.Salesman) } },
            };
        }
        public Calculators GetCalculator(Group group)
        {
            return SalaryDictionary[group];
        }
    }
}
