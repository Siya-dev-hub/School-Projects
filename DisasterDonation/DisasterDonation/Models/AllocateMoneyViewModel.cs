namespace DisasterDonation.Models
{
    public class AllocateMoneyViewModel
    {
        public int DisasterId { get; set; }
        public int MonetaryDonationId { get; set; }
        public IEnumerable<Disaster> ActiveDisasters { get; set; }
        public IEnumerable<MonetaryDonation> UnallocatedMonetaryDonations { get; set; }
    }
}
