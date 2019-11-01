using System;
using System.Collections.Generic;
using MD.Salary.WebApi.Core.Models;

namespace MD.Salary.WebApi.Models
{
    public class Factory
    {
        public struct Calculators
        {
            public CalculatorPersonal Personal;
            public ICalculatorSub Sub;
        }
        private Dictionary<Group, Calculators> SalaryDictionary { get; set; }
        private Dictionary<Group, EmployeeFull> EmployeeDictionary { get; set; }
        private Group Group { get; set; }
        public Factory(Employee employeeDB)
        {
            Enum.TryParse(employeeDB.Group, out Group group);
            Group = group;
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
            EmployeeDictionary = new Dictionary<Group, EmployeeFull> {
                { Group.Employee, new EmployeeFull(employeeDB) },
                { Group.Manager, new EmployeeFull(employeeDB) },
                { Group.Salesman, new EmployeeFull(employeeDB) },
            };
        }
        public Calculators GetCalculator()
        {
            return SalaryDictionary[Group];
        }
        public EmployeeFull GetEmployee()
        {
            return EmployeeDictionary[Group];
        }
    }
}
