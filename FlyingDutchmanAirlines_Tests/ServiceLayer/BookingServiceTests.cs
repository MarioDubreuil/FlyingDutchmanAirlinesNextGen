using FlyingDutchmanAirlines.DatabaseLayer.Models;
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
        mockBookingRepository
            .Setup(r => r.CreateBooking(0, 0))
            .Returns(Task.CompletedTask);

        var mockCustomerRepository = new Mock<CustomerRepository>();
        mockCustomerRepository
            .Setup(r => r.GetCustomerByName("Leo Tolstoy"))
            .Returns(Task.FromResult(new Customer("Leo Tolstoy")));

        var mockFlightRepository = new Mock<FlightRepository>();

        var service = new BookingService(mockBookingRepository.Object, mockCustomerRepository.Object, mockFlightRepository.Object);

        (var result, var exception) = await service.CreateBooking("Leo Tolstoy", 0);

        Assert.IsTrue(result);
        Assert.IsNull(exception);
    }

    [TestMethod]
    [DataRow("", 0)]
    [DataRow(null, 3)]
    [DataRow("abc", -1)]
    public async Task CreateBooking_Failure_InvalidArguments(string customerName, int flightNumber)
    {
        var mockBookingRepository = new Mock<BookingRepository>();
        var mockCustomerRepository = new Mock<CustomerRepository>();
        var mockFlightRepository = new Mock<FlightRepository>();

        var service = new BookingService(mockBookingRepository.Object, mockCustomerRepository.Object, mockFlightRepository.Object);

        (var result, var exception) = await service.CreateBooking(customerName, flightNumber);

        Assert.IsFalse(result);
        Assert.IsNotNull(exception);
    }
}
