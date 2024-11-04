using DisasterDonationAlleviationApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DisasterDonationAlleviationApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<MonetaryDonation> MonetaryDonations { get; set; }
        public DbSet<GoodsDonation> GoodsDonations { get; set; }
        public DbSet<AllocatedFund> AllocatedFunds { get; set; }
        public DbSet<Disaster> Disasters { get; set; }
        public DbSet<DisasterGoodsAllocation> DisasterGoodsAllocations { get; set; }
        public DbSet<AidType> AidTypes { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Goods> Goods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
            modelBuilder.Entity<DisasterAidType>()
                .HasKey(dat => new { dat.DisasterId, dat.AidTypeId });

            modelBuilder.Entity<DisasterAidType>()
                .HasOne(dat => dat.Disaster)
                .WithMany(d => d.DisasterAidTypes)
                .HasForeignKey(dat => dat.DisasterId);

            modelBuilder.Entity<DisasterAidType>()
                .HasOne(dat => dat.AidType)
                .WithMany(a => a.DisasterAidTypes)
                .HasForeignKey(dat => dat.AidTypeId);
        }

    }

}
