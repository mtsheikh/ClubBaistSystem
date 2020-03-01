using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ClubBaistSystem.Domain;
using ClubBaistSystem.TechnicalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ClubBaistSystem.Pages
{
    [BindProperties]
    [Authorize(Roles = "Shareholder,ShareholderSpouse,Associate,Intermediate")]
    public class RecordGolferScores : PageModel
    {
        [Required(ErrorMessage = "None of the fields in the Scorecard can be blank")]
        public List<Round> Rounds { get; set; }
        [Required(ErrorMessage = "Please provide the Name of the Golf Course.")]
        public string Course { get; set; }
        [Required(ErrorMessage = "Please provide the Date for the Scorecard.")]
        public DateTime GameDate { get; set; }

        [TempData]
        public string Alert { get; set; }

        private ClubBaistUser _authenticatedUser;

        private readonly Scorecard _golferScorecard = new Scorecard();
        
        private readonly CBS _requestDirector = new CBS();
        public void OnGet()
        {
            Rounds = new List<Round>();

            for (var i = 1; i <= 18; i++)
            {
                var tempRound = new Round()
                {
                    Hole = i
                };
                Rounds.Add(tempRound);
            }
            _golferScorecard.Rounds = Rounds;
        }

        public ActionResult OnPost()
        {
            _authenticatedUser = ClubBaistUsers.GetUserFromUserName(User.Identity.Name);

            if (!ModelState.IsValid) return Page();

            _golferScorecard.Course = Course;
            _golferScorecard.Date = GameDate;
            _golferScorecard.Rounds = Rounds;

            _golferScorecard.Golfer = (Golfer) _authenticatedUser;

            var result = _requestDirector.RecordPlayerScores(_golferScorecard);

            if (!result) return Page();

            Alert = $"Golfer Score recorded Successfully";
            return RedirectToPage("Index");
        }
    }
}