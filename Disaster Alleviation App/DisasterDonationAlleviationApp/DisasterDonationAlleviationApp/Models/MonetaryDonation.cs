using System.ComponentModel.DataAnnotations;

namespace DisasterDonationAlleviationApp.Models
{
    public class MonetaryDonation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Donation Date is required")]
        [Display(Name = "Donation Date")]
        public DateTime Date { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        [Display(Name = "Donation Amount")]
        [Required(ErrorMessage = "Donation Amount is required")]
        public decimal Amount { get; set; }


        [Display(Name = "Anonymous Donation")]
        public bool IsAnonymous { get; set; }
    }
}
