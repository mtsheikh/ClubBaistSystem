namespace ClubBaistSystem.Domain
{
    public abstract class Golfer : ClubBaistUser
    {
        public abstract string MembershipLevel { get; }
        public abstract string UserType { get; }
    }
}