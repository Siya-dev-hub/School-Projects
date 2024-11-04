using System.Collections.Generic;
using System.Security.AccessControl;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DisasterDonation.Models
{
    public class DisasterDonationDbContext : IdentityDbContext<IdentityUser>
    {

        public DbSet<MonetaryDonation> MonetaryDonations { get; set; }
        public DbSet<GoodsDonation> GoodsDonations { get; set; }
        public DbSet<Disaster> Disasters { get; set; }
        public DbSet<AidType> AidTypes { get; set; }


        public DbSet<Purchase> Purchases { get; set; }

        public DisasterDonationDbContext(DbContextOptions<DisasterDonationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed the database with predefined aid types
            modelBuilder.Entity<AidType>().HasData(
                new AidType { Id = 1, Name = "Food", Description = "Food aid" },
                new AidType { Id = 2, Name = "Medical", Description = "Medical aid" },
                new AidType { Id = 3, Name = "Shelter", Description = "Shelter aid" }
            // Add more as needed
            );
        }
    }
}
