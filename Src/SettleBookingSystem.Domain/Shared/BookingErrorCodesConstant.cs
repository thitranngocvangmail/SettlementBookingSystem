using System;
namespace SettleBookingSystem.Domain.Shared
{
    public static class BookingErrorCodesConstant
    {
        /// <summary>
        /// Missing required field such as name
        /// </summary>
        public const string MissingBookingDetails = "BookingErr:001";

        /// <summary>
        /// Booking time has correct format but is not in working hour
        /// </summary>
        public const string BookingTimeIsOutOfWorkingHour = "BookingErr:002";

        /// <summary>
        /// Booking time has incorrect format
        /// </summary>
        public const string BookingTimeHasInvalidFormat = "BookingErr:003";

        /// <summary>
        /// Bookings at the same time are full
        /// </summary>
        public const string ConflictBooking = "BookingErr:004";
    }
}

