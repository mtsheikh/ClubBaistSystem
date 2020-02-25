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
    public class ReserveTeeTime : PageModel
    {
        private readonly CBS _requestDirector = new CBS();

        public DailyTeeSheet RequestedDailyTeeSheet;
        [TempData] public string Alert { get; set; }

        public List<DateTime> ThisWeek { get; set; }
        private readonly List<DateTime> _generatedWeek = new List<DateTime>();
        
        private void CurrentWeekGeneration()
        {
            for (var i = 0; i < 7; i++)
            {
                _generatedWeek.Add(DateTime.Now.AddDays(i));
            }
            ThisWeek = _generatedWeek;
        }
        
        public void OnGet()
        {
            CurrentWeekGeneration();    
        }

        public void OnPost()
        {
            var selectedDate = DateTime.Parse(Request.Query["date"]);
            RequestedDailyTeeSheet = _requestDirector.FindDailyTeeSheet(selectedDate, ClubBaistUsers.GetUserFromUserName(User.Identity.Name));
        }
    }
}