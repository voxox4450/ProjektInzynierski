using Harmonogram.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Harmonogram.Common.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<WorkBlock> WorkBlocks { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region User

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
                .HasMany(u => u.WorkBlocks)
                .WithOne(w => w.User)
                .HasForeignKey(w => w.UserId);

            #endregion User

            #region Day

            modelBuilder.Entity<Day>()
                .Property(d => d.Name)
                .HasMaxLength(15)
                .IsRequired();

            modelBuilder.Entity<Day>()
                .HasMany(d => d.WorkBlocks)
                .WithOne(w => w.Day)
                .HasForeignKey(w => w.DayId);

            #endregion Day

            #region WorkBlock

            modelBuilder.Entity<WorkBlock>()
                .Property(w => w.Date)
                .IsRequired();

            modelBuilder.Entity<WorkBlock>()
                .Property(w => w.StartHour)
                .IsRequired();

            modelBuilder.Entity<WorkBlock>()
                .Property(w => w.EndHour)
                .IsRequired();

            modelBuilder.Entity<WorkBlock>()
                .Property(w => w.UserId)
                .IsRequired();

            modelBuilder.Entity<WorkBlock>()
                .Property(w => w.DayId)
                .IsRequired();

            #endregion WorkBlock

            #region Schedule

            modelBuilder.Entity<Schedule>()
                .Property(s => s.Name)
                .IsRequired();
            modelBuilder.Entity<Schedule>()
                .Property(s => s.StartDate)
                .IsRequired();
            modelBuilder.Entity<Schedule>()
                .Property(s => s.EndDate)
                .IsRequired();
            modelBuilder.Entity<Schedule>()
                .Property(s => s.ModifiedOn);
            modelBuilder.Entity<Schedule>()
                .Property(s => s.IsArchived)
                .IsRequired();
            modelBuilder.Entity<Schedule>()
                .HasMany(s => s.Users)
                .WithMany(u => u.Schedules);
            modelBuilder.Entity<Schedule>()
                .HasMany(s => s.WorkBlocks)
                .WithOne(w => w.Schedule)
                .HasForeignKey(w => w.ScheduleId);

            #endregion Schedule
        }
    }
}