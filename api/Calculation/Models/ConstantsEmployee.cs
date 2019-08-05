namespace MD.Salary.WebApi.Models
{
    public class ConstantsEmployee
    {
        public ConstantsEmployee(decimal experienceRate, decimal limitRate, decimal subordinateRate)
        {
            ExperienceRate = experienceRate;
            LimitRate = limitRate;
            SubordinateRate = subordinateRate;
        }
        public decimal ExperienceRate { get; private set; }
        public decimal LimitRate { get; private set; }
        public decimal SubordinateRate { get; private set; }
    }
}
