﻿using System.ComponentModel.DataAnnotations.Schema;

namespace MD.Salary.WebApi.Models
{
    public class Employee
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public long HireDate { get; set; }
        public string Group { get; set; }
        [Column(TypeName = Constants.TypeNameDecimalColumn)]
        public decimal SalaryBase { get; set; }
        public long SuperiorID { get; set; }
    }
}