using System;
using System.Collections.Generic;
using ClubBaistSystem.Domain;
using ClubBaistSystem.TechnicalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubBaistSystem.Pages
{
    [Authorize]
    [BindProperties]
    public class ViewPlayerHandicap : PageModel
    {
        private readonly CBS _requestDirector = new CBS();

        [TempData] public string Alert { get; set; }

        public ClubBaistUser authenticatedPlayer { get; set; }
        public decimal PlayerHandicapFactor { get; set; }
        public void OnGet()
        {
            authenticatedPlayer = ClubBaistUsers.GetUserFromUserName(User.Identity.Name);
            PlayerHandicapFactor = _requestDirector.ViewPlayerHandicap(authenticatedPlayer);
        }
    }
}