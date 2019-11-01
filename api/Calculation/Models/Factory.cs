using System.Collections.Generic;
using MD.Salary.WebApi.Calculation.Models;
using MD.Salary.WebApi.Core.Models;

namespace MD.Salary.WebApi.Models
{
    public class Factory : FactoryBase
    {
        public struct Calculators
        {
            public CalculatorPersonal Personal;
            public ICalculatorSub Sub;
        }
        private Dictionary<Group, Calculators> SalaryDictionary { get; set; }
        private Group Group { get; set; }
        public Factory(Employee employeeDB)
        {
            Group = GetGroup(employeeDB.Group);
            SalaryDictionary = new Dictionary<Group, Calculators> {
                { Group.Employee, new Calculators {
                    Sub = new CalculatorSubEmployee(
                        Constants.Employee),
                    Personal = new CalculatorPersonal(
                        Constants.Employee) } },
                { Group.Manager, new Calculators {
                    Sub = new CalculatorSubManager(
                        Constants.Manager),
                    Personal = new CalculatorPersonal(
                        Constants.Manager) } },
                { Group.Salesman, new Calculators {
                    Sub = new CalculatorSubSalesman(
                        Constants.Salesman),
                    Personal = new CalculatorPersonal(
                        Constants.Salesman) } },
            };
        }
        public Calculators GetCalculator()
        {
            return SalaryDictionary[Group];
        }
    }
}
