namespace AdventureJar.Web.Contracts.Models
{
    public class ActivityModel
    {
        public string Id { get; set; }
        
        public string ActivityName { get; set; }

        public string ActivityDescription { get; set; }

        public string ImageUrl { get; set; }

        public bool IsWeekendActivity { get; set; }

        public bool IsPublicHolidayActivity { get; set; }

        public bool IsBadWeatherFriendlyActivity { get; set; }
    }
}
