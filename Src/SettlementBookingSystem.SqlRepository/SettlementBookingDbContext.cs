using System;
using Microsoft.EntityFrameworkCore;
using SettleBookingSystem.Domain.Entities;

namespace SettlementBookingSystem.SqlRepository
{
    public class SettlementBookingDbContext : DbContext
    {
        public DbSet<SettlementBookingEntity> SettlementBookings { get; set; }

        public SettlementBookingDbContext(DbContextOptions<SettlementBookingDbContext> options) : base(options){ }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SettlementBookingEntity>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<SettlementBookingEntity>()
                .HasIndex(e => e.BookingTime);
        }
    }
}

