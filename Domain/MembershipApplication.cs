using System;

namespace ClubBaistSystem.Domain
{
    public class MembershipApplication
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Shareholder1 { get; set; }
        public string Shareholder2 { get; set; }
        public string MembershipType { get; set; }
        public string Occupation { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPostalCode { get; set; }
        public string CompanyCity { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AlternatePhone { get; set; }
        public string ApplicationStatus { get; set; }
    }
}