namespace DisasterDonation.Models
{
    public class PublicInfoViewModel
    {
        public decimal TotalDonations { get; set; }
        public int TotalGoods { get; set; }
        public List<Disaster> ActiveDisasters { get; set; }
    }
}
