using System;
using System.ComponentModel.DataAnnotations;

namespace DisasterDonationAlleviationApp.Models
{
    public class AllocatedFund
    {
        public int Id { get; set; }

        [Required]
        public int DisasterId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime AllocationDate { get; set; }

        public Disaster Disaster { get; set; } // Navigation property
    }
}
