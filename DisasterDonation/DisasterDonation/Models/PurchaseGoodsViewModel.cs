namespace DisasterDonation.Models
{
    public class PurchaseGoodsViewModel
    {

        public IEnumerable<Disaster> Disasters { get; set; } = new List<Disaster>();
        public int SelectedDisasterId { get; set; }
        public List<PredefinedGood> AvailableGoods { get; set; } = new List<PredefinedGood>();
        public List<GoodsPurchaseItem> SelectedGoods { get; set; } = new List<GoodsPurchaseItem>();
        public decimal AvailableFunds { get; set; }

    }
    
}
