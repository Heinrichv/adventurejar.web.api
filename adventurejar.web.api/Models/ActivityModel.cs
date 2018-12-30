using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureJar.Web.Api.Models
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
