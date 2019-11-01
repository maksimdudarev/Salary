using System;
using MD.Salary.WebApi.Models;

namespace MD.Salary.WebApi.Calculation.Models
{
    public class FactoryBase
    {
        public Group GetGroup(string groupAsString)
        {
            Enum.TryParse(groupAsString, out Group groupAsEnum);
            return groupAsEnum;
        }
    }
}
