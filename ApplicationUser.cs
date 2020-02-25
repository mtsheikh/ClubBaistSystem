using Microsoft.AspNetCore.Identity;

namespace ClubBaistSystem
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData] public string FullName { get; set; }
    }
}