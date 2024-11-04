namespace DisasterDonationAlleviationApp.Models
{
    public class AidType
    {
        public int Id { get; set; }
        public string Type { get; set; }

        public ICollection<DisasterAidType> DisasterAidTypes { get; set; }
    }
}
