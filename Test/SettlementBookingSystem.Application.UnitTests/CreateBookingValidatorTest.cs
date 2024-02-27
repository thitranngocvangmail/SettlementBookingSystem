using SettlementBookingSystem.Application.Bookings.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentValidation;
using FluentValidation.TestHelper;
using SettleBookingSystem.Domain.Shared;

namespace SettlementBookingSystem.Application.UnitTests
{
    public class CreateBookingValidatorTest
    {
        CreateBookingValidator Sut;
        public CreateBookingValidatorTest()
        {
            Sut = new CreateBookingValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ValidatorTest_WhenNameIsMissing_ShouldHaveValidationError(string name)
        {
            var model = new CreateBookingCommand()
            {
                Name = name
            };
            var result = await Sut.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(cmd => cmd.Name).WithErrorCode("NotEmptyValidator"); ;
        }

        [Theory]
        [InlineData(null)]
        public async Task ValidatorTest_WhenBookingTimeIsNull_ShouldHaveValidationError(string bookingTime)
        {
            var model = new CreateBookingCommand()
            {
                Name = "John Smith",
                BookingTime = bookingTime
            };
            var result = await Sut.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(cmd => cmd.BookingTime).WithErrorCode("NotEmptyValidator");
        }


        [Theory]
        [InlineData("")]
        [InlineData("a:b")]
        [InlineData("09")]
        public async Task ValidatorTest_WhenBookingTimeHasWrongFormat_ShouldHaveValidationError(string bookingTime)
        {
            var model = new CreateBookingCommand()
            {
                Name = "John Smith",
                BookingTime = bookingTime
            };
            var result = await Sut.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(cmd => cmd.BookingTime).WithErrorCode(BookingErrorCodesConstant.BookingTimeHasInvalidFormat);
        }


        [Theory]
        [InlineData("08:59")]
        [InlineData("17:00")]
        [InlineData("00:00")]
        public async Task ValidatorTest_WhenBookingTimeOutOfWorkingHour_ShouldHaveValidationError(string bookingTime)
        {
            var model = new CreateBookingCommand()
            {
                Name = "John Smith",
                BookingTime = bookingTime
            };
            var result = await Sut.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(cmd => cmd.BookingTime).WithErrorCode(BookingErrorCodesConstant.BookingTimeIsOutOfWorkingHour);
        }

        [Theory]
        [InlineData("09:00")]
        [InlineData("09:59")]
        [InlineData("16:00")]
        [InlineData("16:59")]
        public async Task ValidatorTest_WhenPayloadIsCorrect_ShouldNotHaveValidationError(string bookingTime)
        {
            var model = new CreateBookingCommand()
            {
                Name = "John Smith",
                BookingTime = bookingTime
            };
            var result = await Sut.TestValidateAsync(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
