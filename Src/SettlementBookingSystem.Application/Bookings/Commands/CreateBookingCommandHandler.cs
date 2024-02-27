using AutoMapper;
using FluentValidation;
using MediatR;
using SettleBookingSystem.Domain.Entities;
using SettleBookingSystem.Domain.Shared;
using SettleBookingSystem.Domain.Shared.Exceptions;
using SettlementBookingSystem.Application.Bookings.Dtos;
using SettlementBookingSystem.Application.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SettlementBookingSystem.Application.Bookings.Commands
{
    public class CreateBookingCommandHandler(ISettlementBookingService settlementBookingService) : IRequestHandler<CreateBookingCommand, BookingDto>
    {
        private readonly ISettlementBookingService _settlementBookingService = settlementBookingService;

        public async Task<BookingDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var bookingResult = await _settlementBookingService.CreateBookingAsync(request.Name, DateTimeOffset.Parse(request.BookingTime));
                var bookingDto = new BookingDto()
                {
                    BookingId = bookingResult.Id,
                    Name = bookingResult.CustomerName,
                    BookingTime = bookingResult.BookingTime
                };

                return bookingDto;
            }
            catch(SettlementBookingException bookingException)
            {
                switch(bookingException.Code)
                {
                    case BookingErrorCodesConstant.MissingBookingDetails:
                    case BookingErrorCodesConstant.BookingTimeIsOutOfWorkingHour:
                        throw new ValidationException(bookingException.Code); //TODO: localized error messages for these error codes
                    case BookingErrorCodesConstant.ConflictBooking:
                    throw new ConflictException(bookingException.Code);
                default:
                    throw;
                }
            }
            catch(Exception)
            {
                throw;
            }
            
        }
    }
}
