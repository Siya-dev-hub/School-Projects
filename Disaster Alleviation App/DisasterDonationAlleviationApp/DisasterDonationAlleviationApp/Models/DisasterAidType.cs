namespace DisasterDonationAlleviationApp.Models
{
    public class DisasterAidType
    {
        public int DisasterId { get; set; }
        public Disaster Disaster { get; set; }

        public int AidTypeId { get; set; }
        public AidType AidType { get; set; }
    }
}
