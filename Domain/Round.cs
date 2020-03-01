using System.ComponentModel.DataAnnotations;

namespace ClubBaistSystem.Domain
{
    public class Round
    {
        public int Hole { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "The Score cannot be a negative number.")]
        [Required(ErrorMessage = "None of the fields in the Scorecard can be blank")]
        public int Score { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "The Course Rating cannot be a negative number.")]
        [Required(ErrorMessage = "None of the fields in the Scorecard can be blank")]
        public decimal Rating { get; set; }
        [Required(ErrorMessage = "None of the fields in the Scorecard can be blank")]
        [Range(0, int.MaxValue, ErrorMessage = "The Course Slope cannot be a negative number." )]
        public decimal Slope { get; set; }
    }
}