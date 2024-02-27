using System;
using SettleBookingSystem.Domain.Core;

namespace SettleBookingSystem.Domain.Shared.Exceptions
{
    public class SettlementBookingException : BusinessException
    {
        public SettlementBookingException(string errorCode) : base(errorCode)
        {
        }

        public SettlementBookingException(string errorCode, DateTimeOffset bookingTime) : base(errorCode)
        {
            WithData(nameof(bookingTime), bookingTime);
        }

        public SettlementBookingException(string errorCode, string name) : base(errorCode)
        {
            WithData(nameof(name), name);
        }
    }
}

