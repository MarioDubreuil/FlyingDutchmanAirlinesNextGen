using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class AirportRepository
{
    private readonly FlyingDutchmanAirlinesContext _context = null!;

    public AirportRepository()
    {
    }

    public AirportRepository(FlyingDutchmanAirlinesContext context)
    {
        _context = context;
    }

    public virtual async Task<Airport> GetAirportById(int airportId)
    {
        if (!airportId.IsPositive())
        {
            Console.WriteLine($"Argument Exception in GetAirportById! AirportId = {airportId}");
            throw new ArgumentException("Invalid argument provided");
        }
        return await _context.Airports.FirstOrDefaultAsync(a => a.AirportId == airportId) ?? throw new AirportNotFoundException();
    }
}
