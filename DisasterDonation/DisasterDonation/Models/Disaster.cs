using System.Security.AccessControl;

namespace DisasterDonation.Models
{
    public class Disaster
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? AidTypeId { get; set; }
    }

    public class AidType
    {
        public int Id { get; set; }
        public string Name { get; set; } // e.g., Food, Medical, Shelter
        public string Description { get; set; }
        
    }
}
