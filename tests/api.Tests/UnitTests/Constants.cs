using System.Collections.Generic;
using MD.Salary.WebApi.Core.Models;

namespace MD.Salary.WebApi.Tests.UnitTests
{
    public static class Constants
    {
        // Arrange
        public static readonly long NotExistingId = 2000;
        public static readonly long ExistingId = 1001;
        readonly static string Group = "Guinness";
        readonly static decimal SalaryBase = 12.00M;
        public static readonly long SalaryDate = 1564952400;
        public static readonly Employee NameMissingItem = new Employee()
        {
            Group = Group,
            SalaryBase = SalaryBase
        };
        public static readonly string CreatedItemName = "Guinness Original 6 Pack";
        public static readonly long CreatedItemId = 5000;
        public static readonly Employee CreatedItem = new Employee()
        {
            ID = CreatedItemId,
            Name = CreatedItemName,
            Group = Group,
            SalaryBase = SalaryBase
        };
        public static readonly Employee ExistingItem = new Employee()
        {
            ID = ExistingId,
            Name = CreatedItemName,
        };

        public static List<Employee> GetTestEmployees()
        {
            var items = new List<Employee>
            {
                new Employee() { ID = 1001, Name = "Orange Juice", Group="Orange Tree", SalaryBase = 5.00M },
                new Employee() { ID = 1002, Name = "Diary Milk", Group="Cow", SalaryBase = 4.00M },
                new Employee() { ID = 1003, Name = "Frozen Pizza", Group="Uncle Mickey", SalaryBase = 12.00M }
            };
            return items;
        }
    }
}
