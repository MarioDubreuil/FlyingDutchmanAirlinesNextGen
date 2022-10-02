using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
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
    public async Task CreateBooking_Success_OldCustomer()
    {
        var mockBookingRepository = new Mock<BookingRepository>();
        var mockCustomerRepository = new Mock<CustomerRepository>();
        var mockFlightRepository = new Mock<FlightRepository>();

        mockCustomerRepository
            .Setup(r => r.GetCustomerByName("Leo Tolstoy"))
            .ReturnsAsync(new Customer("Leo Tolstoy") { CustomerId = 0 });

        mockFlightRepository
            .Setup(r => r.GetFlightByFlightNumber(0))
            .ReturnsAsync(new Flight { FlightNumber = 0, Origin = 0, Destination = 1});

        mockBookingRepository
            .Setup(r => r.CreateBooking(0, 0))
            .Returns(Task.CompletedTask);

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

    [TestMethod]
    public async Task CreateBooking_Failure_RepositoryException()
    {
        var mockBookingRepository = new Mock<BookingRepository>();

        mockBookingRepository
            .Setup(r => r.CreateBooking(0, 1))
            .Throws(new ArgumentException());

        mockBookingRepository
            .Setup(r => r.CreateBooking(1, 2))
            .Throws(new CouldNotAddBookingToDatabaseException());

        var mockCustomerRepository = new Mock<CustomerRepository>();

        mockCustomerRepository
            .Setup(r => r.GetCustomerByName("Galileo Galilei"))
            .Returns(Task.FromResult(new Customer("Galileo Galilei") { CustomerId = 0 }));

        mockCustomerRepository
            .Setup(r => r.GetCustomerByName("Eise Eisinga"))
            .Returns(Task.FromResult(new Customer("Eise Eisinga") { CustomerId = 1 }));

        var mockFlightRepository = new Mock<FlightRepository>();

        var service = new BookingService(mockBookingRepository.Object, mockCustomerRepository.Object, mockFlightRepository.Object);

        (var result, var exception) = await service.CreateBooking("Galileo Galilei", 1);

        Assert.IsFalse(result);
        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(ArgumentException));

        (result, exception) = await service.CreateBooking("Eise Eisinga", 2);

        Assert.IsFalse(result);
        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(CouldNotAddBookingToDatabaseException));
    }

    [TestMethod]
    public async Task CreateBooking_Failure_FlightNotInDatabase()
    {
        var mockBookingRepository = new Mock<BookingRepository>();
        var mockCustomerRepository = new Mock<CustomerRepository>();
        var mockFlightRepository = new Mock<FlightRepository>();

        mockCustomerRepository
            .Setup(r => r.GetCustomerByName("Maurits Escher"))
            .Returns(Task.FromResult(new Customer("Maurits Escher") { CustomerId = 0 }));

        mockFlightRepository
            .Setup(r => r.GetFlightByFlightNumber(19))
            .Throws(new FlightNotFoundException());

        var service = new BookingService(mockBookingRepository.Object, mockCustomerRepository.Object, mockFlightRepository.Object);

        (var result, var exception) = await service.CreateBooking("Maurits Escher", 19);

        Assert.IsFalse(result);
        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(FlightNotFoundException));
    }

    [TestMethod]
    public async Task CreateBooking_Failure_CouldNotAddBookingToDatabaseException()
    {
        var mockBookingRepository = new Mock<BookingRepository>();
        var mockCustomerRepository = new Mock<CustomerRepository>();
        var mockFlightRepository = new Mock<FlightRepository>();

        mockCustomerRepository
            .Setup(r => r.GetCustomerByName("Maurits Escher"))
            .ReturnsAsync(new Customer("Maurits Escher") { CustomerId = 0 });

        mockFlightRepository
            .Setup(r => r.GetFlightByFlightNumber(19))
            .ReturnsAsync(new Flight() { FlightNumber = 19, Origin = 0, Destination = 1 });

        mockBookingRepository
            .Setup(r => r.CreateBooking(0, 19))
            .Throws(new CouldNotAddBookingToDatabaseException());

        var service = new BookingService(mockBookingRepository.Object, mockCustomerRepository.Object, mockFlightRepository.Object);

        (var result, var exception) = await service.CreateBooking("Maurits Escher", 19);

        Assert.IsFalse(result);
        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(CouldNotAddBookingToDatabaseException));
    }
}
