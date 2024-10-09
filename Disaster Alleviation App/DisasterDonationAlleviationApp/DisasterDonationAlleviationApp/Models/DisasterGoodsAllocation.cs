namespace DisasterDonationAlleviationApp.Models
{
    public class DisasterGoodsAllocation
    {
        public int Id { get; set; }
        public int DisasterId { get; set; }
        public int GoodsDonationId { get; set; }
        public int Quantity { get; set; }

        public Disaster Disaster { get; set; }
        public GoodsDonation GoodsDonation { get; set; }
    }
}
