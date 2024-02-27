using FluentAssertions;
using Moq;
using SettleBookingSystem.Domain.Core;
using SettleBookingSystem.Domain.Entities;
using SettleBookingSystem.Domain.SettlementBooking;
using SettleBookingSystem.Domain.Shared.Exceptions;

namespace SettlementBooking.Domain.Test
{
    public class SettlementBookingServiceTest
    {
        Mock<ISettlementBookingRepository> _mockedRepo;
        Mock<IGuidGenerator> _mockedGuidGenerator;
        SettlementBookingService Sut;

        public SettlementBookingServiceTest()
        {
            _mockedRepo = new Mock<ISettlementBookingRepository>();
            _mockedGuidGenerator = new Mock<IGuidGenerator>();
            Sut = new SettlementBookingService(_mockedRepo.Object, _mockedGuidGenerator.Object);
        }

        [Fact]
        public async Task CreateBookingAsyncTest_WhenAllValidatorsPass_ThenBookingIsCreated()
        {
            // Arrange
            var testBookingTime = new DateTimeOffset(2024, 3, 1, 9, 20, 0, TimeSpan.Zero);
            var mockedEntityGuid = Guid.Parse("7bbc96b7-360e-46f7-b8db-49293bb8d4f6");
            var mockedReturnedBokingEntity = new SettlementBookingEntity(mockedEntityGuid);
            _mockedRepo.Setup(repo => repo.InsertAsync(It.IsAny<SettlementBookingEntity>())).Returns(Task.FromResult(mockedReturnedBokingEntity));

            // Act
            var result = await Sut.CreateBookingAsync("John Smith", testBookingTime);

            // Assert
            result.Id.Should().Be(mockedEntityGuid);
        }

        [Fact]
        public async Task CreateBookingAsyncTest_WhenThereIsAvaliableBookingSlot_ThenBookingIsCreated()
        {
            // Arrange
            var testBookingTime = new DateTimeOffset(2024, 3, 1, 9, 20, 0, TimeSpan.Zero);
            var mockedEntityGuid = Guid.Parse("7bbc96b7-360e-46f7-b8db-49293bb8d4f6");
            var mockedReturnedBokingEntity = new SettlementBookingEntity(mockedEntityGuid);
            _mockedRepo.Setup(repo => repo.InsertAsync(It.IsAny<SettlementBookingEntity>())).Returns(Task.FromResult(mockedReturnedBokingEntity));
            _mockedRepo.Setup(repo => repo.CountBookingWithinSameHourAsync(testBookingTime)).Returns(Task.FromResult(3));

            // Act
            var result = await Sut.CreateBookingAsync("John Smith", testBookingTime);

            // Assert
            result.Id.Should().Be(mockedEntityGuid);
        }

        // Booking error, missing details
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task CreateBookingAsyncTest_WhenNameIsMissing_ThenValidationIsThrown(string name)
        {
            // Arrange
            var testBookingTime = new DateTimeOffset(2024, 3, 1, 9, 20, 0, TimeSpan.Zero);
          
            // Act
            Func<Task> act = async () => await Sut.CreateBookingAsync(name, testBookingTime); ;

            // Assert
            await act.Should().ThrowAsync<SettlementBookingException>();
            _mockedRepo.Verify(repo => repo.InsertAsync(It.IsAny<SettlementBookingEntity>()), Times.Never());
        }

        // Booking error due to all bookings are full for the given time
        [Theory]
        [InlineData(9, 10)]
        [InlineData(11, 15)]
        [InlineData(13, 01)]
        [InlineData(16, 59)]
        public async Task CreateBookingAsyncTest_WhenBookingsOfTheSameTimeRangeAreFull_ThenValidationIsThrown(int hour, int minute)
        {
            // Arrange
            var testBookingTime = new DateTimeOffset(2024, 3, 1, hour, minute, 0, TimeSpan.Zero);
            _mockedRepo.Setup(repo => repo.CountBookingWithinSameHourAsync(testBookingTime)).Returns(Task.FromResult(4));

            // Act
            Func<Task> act = async () => await Sut.CreateBookingAsync("John Smith", testBookingTime); ;

            // Assert
            await act.Should().ThrowAsync<SettlementBookingException>();
            _mockedRepo.Verify(repo => repo.InsertAsync(It.IsAny<SettlementBookingEntity>()), Times.Never());
        }

    }
}