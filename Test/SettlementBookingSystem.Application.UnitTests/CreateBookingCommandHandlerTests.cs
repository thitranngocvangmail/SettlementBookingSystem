using FluentAssertions;
using Moq;
using SettleBookingSystem.Domain.Entities;
using SettleBookingSystem.Domain.Shared;
using SettleBookingSystem.Domain.Shared.Exceptions;
using SettlementBookingSystem.Application.Bookings.Commands;
using SettlementBookingSystem.Application.Exceptions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SettlementBookingSystem.Application.UnitTests
{
    public class CreateBookingCommandHandlerTests
    {
        private Mock<ISettlementBookingService> _serviceMock;
        private CreateBookingCommandHandler Sut;
        public CreateBookingCommandHandlerTests()
        {
            _serviceMock = new Mock<ISettlementBookingService>();
            Sut = new CreateBookingCommandHandler(_serviceMock.Object);
        }

        [Fact]
        public async Task GivenValidBookingTime_WhenNoErrorFromDomainService_ThenBookingIsAccepted()
        {
            var command = new CreateBookingCommand
            {
                Name = "test",
                BookingTime = "09:15",
            };
            var mockedEntityGuid = Guid.Parse("42187fff-482d-4c5e-b275-f9b65e2458c5");

            _serviceMock.Setup(m => m.CreateBookingAsync(command.Name, DateTimeOffset.Parse(command.BookingTime))).Returns(Task.FromResult(new SettlementBookingEntity(mockedEntityGuid)));

            var result = await Sut.Handle(command, CancellationToken.None);

            result.BookingId.Should().Be(mockedEntityGuid);
        }

        

        [Fact]
        public void GivenValidBookingTime_WheDomainServiceThrowsConflictBookingError_ThenConflictThrown()
        {
            var command = new CreateBookingCommand
            {
                Name = "test",
                BookingTime = "09:15",
            };
            

            _serviceMock.Setup(m => m.CreateBookingAsync(command.Name, DateTimeOffset.Parse(command.BookingTime))).Throws(new SettlementBookingException(BookingErrorCodesConstant.ConflictBooking));


            Func<Task> act = async () => await Sut.Handle(command, CancellationToken.None);

            act.Should().Throw<ConflictException>();
        }

        // This test is validating exception mapping when the validation at the domain layer is failed
        // Domain validator can re-validate the payload e.g name and bookingTime to be more defensive in case the validator from client-side or application layer has been bypassed or is implemented incorrectly.
        [Fact]
        public void GivenValidBookingTime_WhenDomainServiceThrowMissingDetailsERror_ThenValidationExceptionIsThrown()
        {
            var command = new CreateBookingCommand
            {
                Name = "",
                BookingTime = "09:15",
            };

            _serviceMock.Setup(m => m.CreateBookingAsync(command.Name, DateTimeOffset.Parse(command.BookingTime))).Throws(new SettlementBookingException(BookingErrorCodesConstant.MissingBookingDetails));


            Func<Task> act = async () => await Sut.Handle(command, CancellationToken.None);

            act.Should().Throw<FluentValidation.ValidationException>();
        }
    }
}
