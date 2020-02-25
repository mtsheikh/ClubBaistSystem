using System;

namespace ClubBaistSystem.Domain
{
    public abstract class ClubBaistUser : AspNetUsers
    {
        public abstract DateTime regularPlayingHoursBefore { get; }
        public abstract DateTime regularPlayingHoursAfter { get; }
        public abstract DateTime weekendHolidayPlayingHoursAfter { get; }
    }
}