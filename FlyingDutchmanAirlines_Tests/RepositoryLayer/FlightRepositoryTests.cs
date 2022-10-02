using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines_Tests.Stubs;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer;

[TestClass]
public class FlightRepositoryTests
{
    private FlyingDutchmanAirlinesContext _context = null!;
    private FlightRepository _repository = null!;

    [TestInitialize]
    public async Task TestInitialize()
    {
        DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions =
            new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                .UseInMemoryDatabase("FlyingDutchman")
                .Options;
        _context = new FlyingDutchmanAirlinesContext_Stub(dbContextOptions);

        var flight1 = new Flight
        {
            FlightNumber = 1,
            Origin = 1,
            Destination = 2
        };

        var flight2 = new Flight
        {
            FlightNumber = 10,
            Origin = 3,
            Destination = 4
        };

        _context.Flights.Add(flight1);
        _context.Flights.Add(flight2);

        await _context.SaveChangesAsync();

        _repository = new FlightRepository(_context);
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(2)]
    [ExpectedException(typeof(FlightNotFoundException))]
    public async Task GetFlightByflightNumber_Failure_FlightNotFound(int flightNumber)
    {
        await _repository.GetFlightByFlightNumber(flightNumber);
    }

    [TestMethod]
    [DataRow(1, 1, 2)]
    public async Task GetFlightByflightNumber_Success(int flightNumber, int origin, int destination)
    {
        var flight = await _repository.GetFlightByFlightNumber(flightNumber);
        Assert.IsNotNull(flight);

        var dbFlight = await _context.Flights.FirstAsync(f => f.FlightNumber == flightNumber);
        Assert.AreEqual(dbFlight.FlightNumber, flight.FlightNumber);
        Assert.AreEqual(dbFlight.Origin, flight.Origin);
        Assert.AreEqual(dbFlight.Destination, flight.Destination);
    }

    [TestMethod]
    public void GetFlights_Success()
    {
        var flights = _repository.GetFlights();
        Assert.IsNotNull(flights);

        var flight = flights.ElementAt(0);
        Assert.AreEqual(1, flight.FlightNumber);
        Assert.AreEqual(1, flight.Origin);
        Assert.AreEqual(2, flight.Destination);

        flight = flights.ElementAt(1);
        Assert.AreEqual(10, flight.FlightNumber);
        Assert.AreEqual(3, flight.Origin);
        Assert.AreEqual(4, flight.Destination);
    }
}
