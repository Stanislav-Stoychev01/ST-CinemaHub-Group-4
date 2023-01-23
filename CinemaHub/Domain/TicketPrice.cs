using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Domain
{
    public class TicketPrice : Entity
    {
        public DaysOfTheWeek DaysOfTheWeek { get; set; }
        public ScreeningType ScreeningType { get; set; }
        public double Price { get; set; }
    }

    public enum DaysOfTheWeek
    {
        Weekdays,
        Weekend
    }
    
}
