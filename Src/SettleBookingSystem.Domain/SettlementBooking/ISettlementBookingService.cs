using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SettleBookingSystem.Domain.Entities
{
    public interface ISettlementBookingService
    {
        public Task<SettlementBookingEntity> CreateBookingAsync(string customerName, DateTimeOffset bookingTime);
    }
}

