using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SettleBookingSystem.Domain.Entities;
using SettleBookingSystem.Domain.SettlementBooking;

namespace SettlementBookingSystem.SqlRepository.SettlementBooking
{
    public class SettlementBookingRepository : ISettlementBookingRepository
    {
        private readonly SettlementBookingDbContext _context;
        
        public SettlementBookingRepository(SettlementBookingDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<int> CountBookingWithinSameHourAsync(DateTimeOffset currentBookingTime)
        {
            // DOTO: compare property based on offset part and date
            // Only compare hour & time to simplify the implementation as noted in the test requirement.
            return await _context.SettlementBookings.CountAsync(b => b.BookingTime.Hour == currentBookingTime.Hour);
        }


        public async Task<SettlementBookingEntity> InsertAsync(SettlementBookingEntity entity)
        {
            var addedEntity =  await _context.SettlementBookings.AddAsync(entity);
            _context.SaveChanges();
            return addedEntity.Entity;
        }
    }
}

