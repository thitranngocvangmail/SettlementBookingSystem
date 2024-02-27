using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SettleBookingSystem.Domain.Core;
using SettleBookingSystem.Domain.SettlementBooking;
using SettleBookingSystem.Domain.Shared;
using SettleBookingSystem.Domain.Shared.Exceptions;

namespace SettleBookingSystem.Domain.Entities
{
    public class SettlementBookingService(ISettlementBookingRepository bookingRepo, IGuidGenerator guidGenetor) : ISettlementBookingService
    {
        private readonly ISettlementBookingRepository _bookingRepo = bookingRepo;
        private readonly IGuidGenerator _guidGenetor = guidGenetor;


        public async Task<SettlementBookingEntity> CreateBookingAsync(string customerName, DateTimeOffset bookingTime)
        {
            var newBooking = new SettlementBookingEntity(_guidGenetor.NewGuid());
            newBooking.SetCustomerName(customerName);
            newBooking.SetBookingTime(bookingTime);

            var numberOfBookingWithinTheSameHour = await _bookingRepo.CountBookingWithinSameHourAsync(bookingTime);

            // The MaximumBookingPerHour should be configurable in System configuration or Database, using Constant here for quicker implementation for this test
            if (numberOfBookingWithinTheSameHour >= SettlementBookinngConstant.MaximumBookingPerHour)
            {
                throw new SettlementBookingException(BookingErrorCodesConstant.ConflictBooking, bookingTime);
            }

            return await _bookingRepo.InsertAsync(newBooking);
        }

    }
}

