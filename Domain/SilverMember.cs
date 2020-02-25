using System;

namespace ClubBaistSystem.Domain
{
    public abstract class SilverMember : Golfer
    {
        public override string MembershipLevel => "Silver";
        public override DateTime regularPlayingHoursBefore => new DateTime(1, 1, 1, 15, 00, 0);
        public override DateTime regularPlayingHoursAfter => new DateTime(1, 1, 1, 17, 30, 0);
        public override DateTime weekendHolidayPlayingHoursAfter => new DateTime(1, 1, 1, 11, 0, 0);
    }
}