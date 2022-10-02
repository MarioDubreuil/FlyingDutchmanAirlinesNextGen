using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using Moq;

namespace FlyingDutchmanAirlines_Tests.ServiceLayer;

[TestClass]
public class BookingServiceTests
{
    [TestInitialize]
    public async Task TestInitialize()
    {
    }

    [TestMethod]
    public async Task CreateBooking_Success()
    {
        var mockBookingRepository = new Mock<BookingRepository>();
        //mockBookingRepository
        //    .Setup(r => r.CreateBooking(0, 0))
        //    .Returns(Task.CompletedTask);

        var mockCustomerRepository = new Mock<CustomerRepository>();

        var mockFlightRepository = new Mock<FlightRepository>();

        var service = new BookingService(mockBookingRepository.Object, mockCustomerRepository.Object, mockFlightRepository.Object);

        (var result, var exception) = await service.CreateBooking(1, 0);

        Assert.IsTrue(result);
        Assert.IsNull(exception);
    }
}
