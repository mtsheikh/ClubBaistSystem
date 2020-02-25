using System;
namespace ClubBaistSystem.Domain
{
    public class ProShopStaff : ClubBaistUser
    {
        public override DateTime regularPlayingHoursBefore => new DateTime(1, 1, 1, 0, 0, 0);
        public override DateTime regularPlayingHoursAfter => new DateTime(1, 1, 1, 0, 0, 0);
        public override DateTime weekendHolidayPlayingHoursAfter => new DateTime(1, 1, 1, 0, 0, 0);
    }
}