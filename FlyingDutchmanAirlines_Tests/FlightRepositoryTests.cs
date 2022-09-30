using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines_Tests.Stubs;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests;

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

        var flight = new Flight
        {
            FlightNumber = 1,
            Origin = 1,
            Destination = 2
        };
        _context.Flights.Add(flight);
        await _context.SaveChangesAsync();

        _repository = new FlightRepository(_context);
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    [DataRow(-1, 0, 0)]
    [DataRow(0, -1, 0)]
    [DataRow(0, 0, -1)]
    [DataRow(-1, -1, -1)]
    [ExpectedException(typeof(ArgumentException))]
    public async Task GetFlightByflightNumber_Failure_InvalidArguments(int flightNumber, int origin, int destination)
    {
        await _repository.GetFlightByFlightNumber(flightNumber, origin, destination);
    }

    [TestMethod]
    [DataRow(0, 0, 0)]
    [DataRow(1, 1, 1)]
    [ExpectedException(typeof(FlightNotFoundException))]
    public async Task GetFlightByflightNumber_Failure_FlightNotFound(int flightNumber, int origin, int destination)
    {
        await _repository.GetFlightByFlightNumber(flightNumber, origin, destination);
    }

    [TestMethod]
    [DataRow(1, 1, 2)]
    public async Task GetFlightByflightNumber_Success(int flightNumber, int origin, int destination)
    {
        var flight = await _repository.GetFlightByFlightNumber(flightNumber, origin, destination);
        Assert.IsNotNull(flight);

        var dbFlight = await _context.Flights.FirstAsync(f => f.FlightNumber == flightNumber);
        Assert.AreEqual(dbFlight.FlightNumber, flight.FlightNumber);
        Assert.AreEqual(dbFlight.Origin, flight.Origin);
        Assert.AreEqual(dbFlight.Destination, flight.Destination);
    }
}
