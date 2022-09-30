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
    public async Task TestInitialize()
    {
        DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions =
            new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                .UseInMemoryDatabase("FlyingDutchman")
                .Options;
        _context = new FlyingDutchmanAirlinesContext_Stub(dbContextOptions);

        var airports = new SortedList<string, Airport>
        {
            {
                "GOH",
                new Airport
                {
                    AirportId = 0,
                    City = "Nuuk",
                    Iata = "GOH"
                }
            },
            {
                "PHX",
                new Airport
                {
                    AirportId = 1,
                    City = "Phoenix",
                    Iata = "PHX"
                }
            },
            {
                "DDH",
                new Airport
                {
                    AirportId = 2,
                    City = "Bennington",
                    Iata = "DDH"
                }
            },
            {
                "RDU",
                new Airport
                {
                    AirportId = 3,
                    City = "Raleigh-Durham",
                    Iata = "RDU"
                }
            }
        };

        _context.Airports.AddRange(airports.Values);
        await _context.SaveChangesAsync();

        _repository = new AirportRepository(_context);
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    public async Task GetAirportById_Success()
    {
        Airport airport = await _repository.GetAirportById(0);
        Assert.IsNotNull(airport);
        Assert.AreEqual(airport.AirportId, 0);
        Assert.AreEqual(airport.City, "Nuuk");
        Assert.AreEqual(airport.Iata, "GOH");
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(-5)]
    [ExpectedException(typeof(ArgumentException))]
    public async Task GetAirportById_Failure_InvalidArgument(int airportId)
    {
        using (StringWriter outputString = new StringWriter())
        {
            Console.SetOut(outputString);
            try
            {
                await _repository.GetAirportById(airportId);
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(outputString.ToString().Contains($"Argument Exception in GetAirportById! AirportId = {airportId}"));
                throw;
            }
        }
    }
}
