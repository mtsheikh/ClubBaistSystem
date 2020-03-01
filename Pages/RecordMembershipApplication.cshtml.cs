using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using ClubBaistSystem.Domain;
using ClubBaistSystem.TechnicalServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClubBaistSystem.Pages
{
    // TODO: Index page configured to show alert messages
    // TODO: Implement a drop-down box for Membership Type    
    [BindProperties]
    public class RecordMembershipApplication : PageModel
    {
        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(25)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(25)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Address is required.")]
        [StringLength(25)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Postal Code is required.")]
        [RegularExpression(@"[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVXY]\s[0-9][ABCEGHJKLMNPRSTVXY][0-9]"
            , ErrorMessage = "Postal Code should be in A0A 0A0 Format")]        [StringLength(7)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        
        [Required(ErrorMessage = "City is required.")]
        [StringLength(25)]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Date Of Birth is required.")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        
        [Required(ErrorMessage = "Shareholder #1 is required.")]
        [StringLength(25)]
        [Display(Name = "Shareholder #1")]
        public string Shareholder1 { get; set; }

        [Required(ErrorMessage = "Shareholder #2 is required.")]
        [StringLength(25)]
        [Display(Name = "Shareholder #2")]
        public string Shareholder2 { get; set; }
        
        [Required(ErrorMessage = "Membership Type is required.")]
        [StringLength(15)]
        [Display(Name = "Membership Type")]
        public string MembershipType { get; set; }

        [Required(ErrorMessage = "Occupation is required.")]
        [StringLength(25)]
        [Display(Name = "Occupation")]
        public string Occupation { get; set; }

        [Required(ErrorMessage = "Company Name is required.")]
        [StringLength(25)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        
        [Required(ErrorMessage = "Company Address is required.")]
        [StringLength(25)]
        [Display(Name = "Company Address")]
        public string CompanyAddress { get; set; }
        
        [Required(ErrorMessage = "Company Postal Code is required.")]
        [RegularExpression(@"[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVXY]\s[0-9][ABCEGHJKLMNPRSTVXY][0-9]"
                           , ErrorMessage = "Postal Code should be in A0A 0A0 Format")]
        [StringLength(7)]
        [Display(Name = "Company Postal Code")]
        public string CompanyPostalCode { get; set; }
        
        [Required(ErrorMessage = "Company City is required.")]
        [StringLength(25)]
        [Display(Name = "Company City")]
        public string CompanyCity { get; set; }
        
        [Required(ErrorMessage = "Email is required.")]
        [StringLength(25)]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Primary Phone Number is required.")]
        [StringLength(10)]
        [RegularExpression(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$"
            , ErrorMessage = "Phone Number should be in 1234567890 Format")]
        [Display(Name = "Primary Phone Number")]
        public string Phone { get; set; }

        [StringLength(10)]
        [RegularExpression(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$"
                        , ErrorMessage = "Phone Number should be in 1234567890 Format")]
        [Display(Name = "Alternate Phone Number")]
        public string AlternatePhone { get; set; }
      
        public string Alert { get; set; }
        public readonly CBS RequestDirector = new CBS();

        // public SelectList MembershipTypeSelectList { get; set; }

        public void OnGet()
        {
        //     MembershipTypeSelectList = new SelectList(
        //     new List<SelectListItem>
        //     {
        //         new SelectListItem {Text = "Shareholder", Value = "Shareholder"},
        //         new SelectListItem {Text = "Associate", Value = "Associate"}
        //     });
        }
        
        public ActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var newMembershipApplication = new MembershipApplication();
                newMembershipApplication.LastName = LastName;
                newMembershipApplication.FirstName = FirstName;
                newMembershipApplication.Address = Address;
                newMembershipApplication.PostalCode = PostalCode;
                newMembershipApplication.City = City;
                newMembershipApplication.DateOfBirth = DateOfBirth;
                newMembershipApplication.Shareholder1 = Shareholder1;
                newMembershipApplication.Shareholder2 = Shareholder2;
                newMembershipApplication.MembershipType = MembershipType;
                newMembershipApplication.Occupation = Occupation;
                newMembershipApplication.CompanyName = CompanyName;
                newMembershipApplication.CompanyAddress = CompanyAddress;
                newMembershipApplication.CompanyPostalCode = CompanyPostalCode;
                newMembershipApplication.CompanyCity = CompanyCity;
                newMembershipApplication.Email = Email;
                newMembershipApplication.Phone = Phone;
                newMembershipApplication.AlternatePhone = AlternatePhone; 

                var result = RequestDirector.RecordMembershipApplication(newMembershipApplication);

                if (!result) return Page();

                Alert = $"Membership Application Record submitted successfully for {FirstName} {LastName}.";
                return RedirectToPage("Index");
            }

            return Page();
        }
    }
}