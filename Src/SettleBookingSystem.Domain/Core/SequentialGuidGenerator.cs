using System;
using Taikandi;

namespace SettleBookingSystem.Domain.Core
{
    public class SequentialGuidGenerator : IGuidGenerator
    {
        public Guid NewGuid()
        {
            return SequentialGuid.NewGuid();
        }
    }
}

