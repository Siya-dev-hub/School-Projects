namespace DisasterDonationAlleviationApp.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public int GoodsId { get; set; }
        public int Quantity { get; set; }
        public int DisasterId { get; set; }
        public Goods Goods { get; set; }
        public Disaster Disaster { get; set; }
    }
}
