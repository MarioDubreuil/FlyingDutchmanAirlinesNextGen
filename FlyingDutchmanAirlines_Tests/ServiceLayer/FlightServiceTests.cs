using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using Moq;

namespace FlyingDutchmanAirlines_Tests.ServiceLayer;

[TestClass]
public class FlightServiceTests
{
    [TestMethod]
    public async Task GetFlights_Success()
    {
        var flightInDatabse = new Flight
        {
            FlightNumber = 148,
            Origin = 31,
            Destination = 92
        };

        var mockReturn = new Queue<Flight>(1);
        mockReturn.Enqueue(flightInDatabse);

        var mockFlightRepository = new Mock<FlightRepository>();
        var mockAirportRepository = new Mock<AirportRepository>();

        mockFlightRepository
            .Setup(r => r.GetFlights())
            .Returns(mockReturn);

        mockAirportRepository
            .Setup(r => r.GetAirportById(31))
            .ReturnsAsync(new Airport
            {
                AirportId = 31,
                City = "Mexico City",
                Iata = "MEX"
            });

        mockAirportRepository
            .Setup(r => r.GetAirportById(92))
            .ReturnsAsync(new Airport
            {
                AirportId = 92,
                City = "Ulaanbaataar",
                Iata = "UBN"
            });

        var service = new FlightService(mockFlightRepository.Object, mockAirportRepository.Object);

        await foreach(var flightView in service.GetFlights())
        {
            Assert.IsNotNull(flightView);
            Assert.AreEqual("148", flightView.FlightNumber);
            Assert.AreEqual("Mexico City", flightView.Origin.City);
            Assert.AreEqual("MEX", flightView.Origin.Code);
            Assert.AreEqual("Ulaanbaataar", flightView.Destination.City);
            Assert.AreEqual("UBN", flightView.Destination.Code);
        }
    }

    [TestMethod]
    [ExpectedException(typeof(AirportNotFoundException))]
    public async Task GetFlights_Failure_AirportNotFoundException()
    {
        var flightInDatabse = new Flight
        {
            FlightNumber = 148,
            Origin = 31,
            Destination = 92
        };

        var mockReturn = new Queue<Flight>(1);
        mockReturn.Enqueue(flightInDatabse);

        var mockFlightRepository = new Mock<FlightRepository>();
        var mockAirportRepository = new Mock<AirportRepository>();

        mockFlightRepository
            .Setup(r => r.GetFlights())
            .Returns(mockReturn);

        mockAirportRepository
            .Setup(r => r.GetAirportById(31))
            .ThrowsAsync(new AirportNotFoundException());

        var service = new FlightService(mockFlightRepository.Object, mockAirportRepository.Object);

        await foreach (var flightView in service.GetFlights())
        {
            ;
        }
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task GetFlights_Failure_ArgumentException()
    {
        var flightInDatabse = new Flight
        {
            FlightNumber = 148,
            Origin = 31,
            Destination = 92
        };

        var mockReturn = new Queue<Flight>(1);
        mockReturn.Enqueue(flightInDatabse);

        var mockFlightRepository = new Mock<FlightRepository>();
        var mockAirportRepository = new Mock<AirportRepository>();

        mockFlightRepository
            .Setup(r => r.GetFlights())
            .Returns(mockReturn);

        mockAirportRepository
            .Setup(r => r.GetAirportById(31))
            .ThrowsAsync(new NullReferenceException());

        var service = new FlightService(mockFlightRepository.Object, mockAirportRepository.Object);

        await foreach (var flightView in service.GetFlights())
        {
            ;
        }
    }

    [TestMethod]
    public async Task GetFlightByFlightNumber_Success()
    {
        var mockFlightRepository = new Mock<FlightRepository>();
        var mockAirportRepository = new Mock<AirportRepository>();

        mockFlightRepository
            .Setup(r => r.GetFlightByFlightNumber(148))
            .ReturnsAsync(new Flight
            {
                FlightNumber = 148,
                Origin = 31,
                Destination = 92
            });

        mockAirportRepository
            .Setup(r => r.GetAirportById(31))
            .ReturnsAsync(new Airport
            {
                AirportId = 31,
                City = "Mexico City",
                Iata = "MEX"
            });

        mockAirportRepository
            .Setup(r => r.GetAirportById(92))
            .ReturnsAsync(new Airport
            {
                AirportId = 92,
                City = "Ulaanbaataar",
                Iata = "UBN"
            });

        var service = new FlightService(mockFlightRepository.Object, mockAirportRepository.Object);

        var flightView = await service.GetFlightByFlightNumber(148);

        Assert.IsNotNull(flightView);
        Assert.AreEqual("148", flightView.FlightNumber);
        Assert.AreEqual("Mexico City", flightView.Origin.City);
        Assert.AreEqual("MEX", flightView.Origin.Code);
        Assert.AreEqual("Ulaanbaataar", flightView.Destination.City);
        Assert.AreEqual("UBN", flightView.Destination.Code);
    }

    [TestMethod]
    [ExpectedException(typeof(FlightNotFoundException))]
    public async Task GetFlightByFlightNumber_Failure_FlightNotFoundException()
    {
        var mockFlightRepository = new Mock<FlightRepository>();
        var mockAirportRepository = new Mock<AirportRepository>();

        mockFlightRepository
            .Setup(r => r.GetFlightByFlightNumber(148))
            .Throws(new FlightNotFoundException());

        var service = new FlightService(mockFlightRepository.Object, mockAirportRepository.Object);

        var flightView = await service.GetFlightByFlightNumber(148);
    }
}
