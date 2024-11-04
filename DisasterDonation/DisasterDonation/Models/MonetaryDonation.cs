using System.ComponentModel.DataAnnotations;

namespace DisasterDonation.Models
{
    public class MonetaryDonation
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }
        [Required]
        public bool IsAnonymous { get; set; }

        public int? AllocatedDisasterId { get; set; }
        public Disaster AllocatedDisaster { get; set; }
    }
}
