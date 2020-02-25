using System;

namespace ClubBaistSystem.Domain
{
    public abstract class BronzeMember : Golfer
    {
        public override string MembershipLevel => "Bronze";
        public override DateTime regularPlayingHoursBefore => new DateTime(1, 1, 1, 15, 0, 0);
        public override DateTime regularPlayingHoursAfter => new DateTime(1, 1, 1, 18, 0, 0);
        public override DateTime weekendHolidayPlayingHoursAfter => new DateTime(1, 1, 1, 13, 0, 0);
    }
}