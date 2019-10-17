namespace MD.Salary.WebApi.Core.Models
{
    public class User
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public long Role { get; set; }
    }
}