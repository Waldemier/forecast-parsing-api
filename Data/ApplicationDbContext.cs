using ForecastAPI.Data.Entities;
using ForecastAPI.Data.Seed;
using Microsoft.EntityFrameworkCore;

namespace ForecastAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
            base(options)
        {

        }

        public DbSet<History> History { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<RegisterConfirm> RegisterConfirms { get; set; }
        public DbSet<VerifyPassword> VerifyPasswords { get; set; }

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

                entity.HasOne(e => e.User)
                    .WithMany(u => u.History)
                    .HasForeignKey(e => e.UserId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Email)
                    .IsUnique();
                
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40);
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(120);
                entity.Property(e => e.Password)
                    .IsRequired();
                entity.Property(e => e.Role)
                    .IsRequired();
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Token)
                    .IsUnique();
                
                entity.Property(e => e.Token)
                    .IsRequired();
                entity.Property(e => e.ExpiryTime)
                    .IsRequired();

                entity.HasOne(e => e.User)
                    .WithOne()
                    .HasForeignKey<RefreshToken>(e => e.UserId);
            });

            modelBuilder.Entity<RegisterConfirm>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Token)
                    .IsUnique();
                entity.HasOne(e => e.User)
                    .WithOne(u => u.RegisterConfirm)
                    .HasForeignKey<RegisterConfirm>(e => e.UserId);
            });
            
            modelBuilder.Entity<VerifyPassword>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Token)
                    .IsUnique();
                entity.Property(e => e.ExpiryTime);
                entity.HasOne(e => e.User)
                    .WithOne(u => u.VerifyPassword)
                    .HasForeignKey<VerifyPassword>(e => e.UserId);
            });
            
            modelBuilder.ApplyConfiguration(new SeedUsers());
        }
    }
}