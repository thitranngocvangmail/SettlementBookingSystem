using System;
using SettleBookingSystem.Domain.Core;
using SettleBookingSystem.Domain.Shared;
using SettleBookingSystem.Domain.Shared.Exceptions;

namespace SettleBookingSystem.Domain.Entities
{
    public class SettlementBookingEntity : Entity<Guid>
    {
        public SettlementBookingEntity()
        {
        }

        public SettlementBookingEntity(Guid id) : base(id)
        {
        }

        /// <summary>
        /// Customer name
        /// </summary>
        public string CustomerName { get; private set; }

        public DateTimeOffset BookingTime { get; private set; }

        public SettlementBookingEntity SetCustomerName(string name)
        {
            // Simple domain model validation can be implemented in entity setter.
            if(string.IsNullOrEmpty(name))
            {
                throw new SettlementBookingException(BookingErrorCodesConstant.MissingBookingDetails, name);
            }
            CustomerName = name;
            return this;
        }

        public SettlementBookingEntity SetBookingTime(DateTimeOffset bookingTime)
        {
            // Validate booking time is in acceptable range, e.g from 9 AM to 4PM
            BookingTime = bookingTime;
            return this;
        }
    }
}

