using NuGet.Packaging.Signing;
using System.Collections.Generic;

namespace DisasterDonationAlleviationApp.Models
{
    public class PurchaseGoodsViewModel
    {
        public IEnumerable<Goods> Goods { get; set; }
        public IEnumerable<Disaster> Disasters { get; set; }
        public int SelectedDisasterId { get; set; }
        public IEnumerable<GoodsPurchaseItem> SelectedGoods { get; set; }
        public decimal AvailableMoney { get; set; }
    }
    public class GoodsPurchaseItem
    {
        public int GoodsId { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
    }
}
