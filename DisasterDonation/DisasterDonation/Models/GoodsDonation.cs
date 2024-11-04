namespace DisasterDonation.Models
{
    public class GoodsDonation
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfItems { get; set; }
        public decimal Cost { get; set; }
        public string Category { get; set; } // e.g., Clothes, Non-perishable foods
        public string Description { get; set; }
        public bool IsAnonymous { get; set; }
        // New Property
        public int? AllocatedDisasterId { get; set; }
        public Disaster AllocatedDisaster { get; set; }

        public int? DisasterId { get; set; }
        public Disaster Disaster { get; set; }
        
    }
}
