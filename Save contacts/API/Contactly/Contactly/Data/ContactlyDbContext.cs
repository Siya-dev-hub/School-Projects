using Microsoft.EntityFrameworkCore;
using Contactly.Models.Domain;
namespace Contactly.Data
{
    public class ContactlyDbContext: DbContext 
    {
        public ContactlyDbContext(DbContextOptions options) : base(options) 
        {
            
        }
        public DbSet<Contact> Contacts { get; set; }
    }
}
