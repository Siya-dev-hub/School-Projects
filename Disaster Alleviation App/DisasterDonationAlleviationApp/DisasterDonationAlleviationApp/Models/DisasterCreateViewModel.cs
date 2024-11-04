using System.Collections.Generic;
namespace DisasterDonationAlleviationApp.Models
{
    public class DisasterCreateViewModel
    {
        public Disaster Disaster { get; set; } // The Disaster object to be created
        public IEnumerable<AidType> AidTypes { get; set; } // List of available aid types
        public List<int> SelectedAidTypes { get; set; }
    }
}
