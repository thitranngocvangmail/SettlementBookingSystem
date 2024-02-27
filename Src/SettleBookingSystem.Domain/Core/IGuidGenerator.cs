using System;

namespace SettleBookingSystem.Domain.Core
{
    public interface IGuidGenerator
    {
        public Guid NewGuid();
    }
}

