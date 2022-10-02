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
    [DataRow(-1)]
    [ExpectedException(typeof(ArgumentException))]
    public async Task GetFlightByflightNumber_Failure_InvalidArguments(int flightNumber)
    {
        await _repository.GetFlightByFlightNumber(flightNumber);
    }

    [TestMethod]
    [DataRow(0)]
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
}
