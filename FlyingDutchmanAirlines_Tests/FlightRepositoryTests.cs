using FlyingDutchmanAirlines.DatabaseLayer;
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
}
