using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SettleBookingSystem.Domain.Entities;

namespace SettleBookingSystem.Domain.SettlementBooking
{
    public interface ISettlementBookingRepository
    {
        public Task<SettlementBookingEntity> InsertAsync(SettlementBookingEntity entity);
        public Task<int> CountBookingWithinSameHourAsync(DateTimeOffset currentBookingTime);
    }
}

