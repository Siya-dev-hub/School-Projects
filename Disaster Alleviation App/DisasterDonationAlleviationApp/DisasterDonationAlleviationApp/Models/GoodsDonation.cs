namespace DisasterDonationAlleviationApp.Models
{
    public class GoodsDonation
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfItems { get; set; }
        public string Category { get; set; } // e.g., Clothes, Non-perishable foods
        public string Description { get; set; }
        public bool IsAnonymous { get; set; }
    }
}
