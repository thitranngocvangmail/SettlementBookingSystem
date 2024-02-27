using System;

namespace SettlementBookingSystem.Application.Bookings.Dtos
{
    public class BookingDto
    {
        public BookingDto()
        {
            
        }

        public Guid BookingId { get; set; }

        public string Name { get; set; }

        public DateTimeOffset BookingTime { get; set; }
    }
}
