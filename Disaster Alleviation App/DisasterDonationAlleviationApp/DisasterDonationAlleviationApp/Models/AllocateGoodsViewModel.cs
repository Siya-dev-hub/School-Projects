namespace DisasterDonationAlleviationApp.Models
{
    public class AllocateGoodsViewModel
    {
        public int SelectedDisasterId { get; set; }
        public int SelectedGoodsId { get; set; }
        public int Quantity { get; set; }

        public IEnumerable<Disaster> Disasters { get; set; }
        public IEnumerable<GoodsDonation> GoodsDonations { get; set; }
    }
}
