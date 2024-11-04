namespace DisasterDonationAlleviationApp.Models
{
    public class Disaster
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public ICollection<DisasterAidType> DisasterAidTypes { get; set; } = new List<DisasterAidType>();
        
    }
}
