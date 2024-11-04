namespace DisasterDonation.Models
{
    public class AllocateGoodsViewModel
    {
        public int GoodsDonationId { get; set; }
        public int DisasterId { get; set; }

        public IEnumerable<Disaster> ActiveDisasters { get; set; }
        public IEnumerable<GoodsDonation> UnallocatedGoodsDonations { get; set; }
    }
}
