using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines_Tests.Stubs;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests;

[TestClass]
public class AirportRepositoryTests
{
    private FlyingDutchmanAirlinesContext _context = null!;
    private AirportRepository _repository = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions =
            new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                .UseInMemoryDatabase("FlyingDutchman")
                .Options;
        _context = new FlyingDutchmanAirlinesContext_Stub(dbContextOptions);

        _repository = new AirportRepository(_context);
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    public async Task GetAirportById_Success()
    {
        Airport airport = await _repository.GetAirportById(0);
        Assert.IsNotNull(airport);
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(-5)]
    [ExpectedException(typeof(ArgumentException))]
    public async Task GetAirportById_Failure_InvalidArgument(int airportId)
    {
        await _repository.GetAirportById(airportId);
    }
}
