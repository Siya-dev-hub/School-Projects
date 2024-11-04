using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRCertificationManagementSystem.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Certification is required")]
        public string Certification { get; set; }

        [Required(ErrorMessage = "Certification expiration date is required")]
        [Display(Name = "Certification Expiration Date")]
        [DataType(DataType.Date)]
        public DateTime CertificationExpirationDate { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        // Make CertificationPdfPath optional
        [Display(Name = "Certification PDF")]
        public string? CertificationPdfPath { get; set; }

        // Add a NotMapped property for the file upload
        [NotMapped]
        [Display(Name = "Upload Certification PDF")]
        public IFormFile? CertificationFile { get; set; }
    }

    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
