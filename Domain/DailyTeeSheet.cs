using System;
using System.Collections.Generic;

namespace ClubBaistSystem.Domain
{
    public class DailyTeeSheet
    {
        public DailyTeeSheet(List<TeeTime> teeTimes)
        {
            TeeTimes = teeTimes;
        }

        public DateTime Date { get; set; }
        public List<TeeTime> TeeTimes { get; set; }
    }
}