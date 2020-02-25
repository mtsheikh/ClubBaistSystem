using System;

namespace ClubBaistSystem.Domain
{
    public abstract class GoldMember : Golfer
    {
        public override string MembershipLevel => "Gold";
        public override DateTime regularPlayingHoursBefore => new DateTime(1, 1, 1, 0, 0, 0);
        public override DateTime regularPlayingHoursAfter => new DateTime(1, 1, 1, 0, 0, 0);
        public override DateTime weekendHolidayPlayingHoursAfter => new DateTime(1, 1, 1, 0, 0, 0);
    }
}