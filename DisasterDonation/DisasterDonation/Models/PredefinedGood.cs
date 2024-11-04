namespace DisasterDonation.Models
{
    public class PredefinedGood
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal UnitCost { get; set; }
        public int AvailableQuantity { get; set; }
    }

    public class GoodsPurchaseItem
    {
        public int GoodsId { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
    }
}
