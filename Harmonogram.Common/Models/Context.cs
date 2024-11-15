using Harmonogram.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Harmonogram.Common.Models
{
    public class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Users
            modelBuilder.Entity<User>()
                .Property(u => u.IsAdmin)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.LastName)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.Mail)
                .HasMaxLength(24)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .HasMaxLength(48)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.PaymentPerHour)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.AccountNumber)
                .HasMaxLength(26)
                .IsRequired();

        }
    }
}
