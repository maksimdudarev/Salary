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
                        Constants.Employee.SubordinateRate),
                    Personal = new CalculatorsClass(
                        Constants.Employee).Personal } },
                {Group.Manager, new Calculators {
                    Sub = new CalculatorSubManager(
                        Constants.Manager.SubordinateRate),
                    Personal = new CalculatorsClass(
                        Constants.Manager).Personal } },
                {Group.Salesman, new Calculators {
                    Sub = new CalculatorSubSalesman(
                        Constants.Salesman.SubordinateRate),
                    Personal = new CalculatorsClass(
                        Constants.Salesman).Personal } },
            };
        }
        public Calculators GetCalculator(Group group)
        {
            return SalaryDictionary[group];
        }
    }
    class CalculatorsClass
    {
        public CalculatorPersonal Personal { get; set; }
        public CalculatorsClass(ConstantsEmployee rates)
        {
            Personal = new CalculatorPersonal(rates.ExperienceRate, rates.LimitRate);
        }
    }
}
