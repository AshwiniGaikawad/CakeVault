using CakeVault.Models;
using Microsoft.EntityFrameworkCore;

namespace CakeVault
{
    public class CakeVaultDBContext: DbContext
    {
        public CakeVaultDBContext(DbContextOptions<CakeVaultDBContext> options) : base(options) { }

        public DbSet<Cake> Cakes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cake>()
                .HasIndex(c => c.Name)
                .IsUnique();
        }
    }
}
