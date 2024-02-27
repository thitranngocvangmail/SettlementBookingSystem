using FluentValidation;
using SettleBookingSystem.Domain.Shared;
using System;
using System.Globalization;

namespace SettlementBookingSystem.Application.Bookings.Commands
{
    public class CreateBookingValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingValidator()
        {
            RuleFor(b => b.Name).NotEmpty();
            RuleFor(b => b.BookingTime).NotEmpty().Matches("[0-9]{1,2}:[0-9][0-9]").WithErrorCode(BookingErrorCodesConstant.BookingTimeHasInvalidFormat).WithMessage("Booking time has incorrect format. Accepted format is hh:mm");
            RuleFor(b => b.BookingTime).Must(IsInWorkingHours).WithErrorCode(BookingErrorCodesConstant.BookingTimeIsOutOfWorkingHour).WithMessage("Booking time must be from 09:00 AM to 04:00 PM");
            } 
        

        // This validation is a kind of business validation so it can also be defined in the Application layer
        // Working hour should be defined from configuration or DB if this is configurable per tenant or customer
        private bool IsInWorkingHours(string timeStringInput)
        {
            if (!DateTime.TryParse(timeStringInput, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out var parsedValue))
                return false;

            return parsedValue.Hour >= 9 && parsedValue.Hour <= 16;

        }
    }
}
