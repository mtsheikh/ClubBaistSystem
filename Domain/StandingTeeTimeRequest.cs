using System;

namespace ClubBaistSystem.Domain
{
    public class StandingTeeTimeRequest
    {
        public DateTime Time { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DayOfWeek { get; set; }
        public ClubBaistUser Shareholder1 { get; set; }
        public ClubBaistUser Shareholder2 { get; set; }
        public ClubBaistUser Shareholder3 { get; set; }
        public ClubBaistUser Shareholder4 { get; set; }
        public string BookerId { get; set; }
    }
}