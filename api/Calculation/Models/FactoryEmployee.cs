using System.Collections.Generic;
using MD.Salary.WebApi.Calculation.Models;
using MD.Salary.WebApi.Core.Models;

namespace MD.Salary.WebApi.Models
{
    public class FactoryEmployee : FactoryBase
    {
        private Dictionary<Group, EmployeeFull> EmployeeDictionary { get; set; }
        private Group Group { get; set; }
        public FactoryEmployee(Employee employeeDB)
        {
            Group = GetGroup(employeeDB.Group);
            EmployeeDictionary = new Dictionary<Group, EmployeeFull> {
                { Group.Employee, new EmployeeSimple(employeeDB) },
                { Group.Manager, new EmployeeManager(employeeDB) },
                { Group.Salesman, new EmployeeSalesman(employeeDB) },
            };
        }
        public EmployeeFull GetEmployee()
        {
            return EmployeeDictionary[Group];
        }
    }
}
