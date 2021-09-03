using ForecastAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ForecastAPI.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):
            base(options)
        {
            
        }

        private DbSet<History> History { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<History>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Temperature)
                    .IsRequired();
                entity.Property(e => e.Date)
                    .IsRequired();
                entity.Property(e => e.City)
                    .IsRequired();
            });
        }
    }
}