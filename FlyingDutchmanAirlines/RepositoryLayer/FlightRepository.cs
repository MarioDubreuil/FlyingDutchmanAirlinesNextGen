using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class FlightRepository
{
    private readonly FlyingDutchmanAirlinesContext _context = null!;

    public FlightRepository()
    {
    }

	public FlightRepository(FlyingDutchmanAirlinesContext context)
	{
		_context = context;
	}

	public async Task<Flight> GetFlightByFlightNumber(int flightNumber)
	{
        if (!flightNumber.IsPositive())
        {
            Console.WriteLine($"Argument Exception in GetFlightByFlightNumber! FlightNumber = {flightNumber}");
            throw new FlightNotFoundException();
        }
        return await _context.Flights.FirstOrDefaultAsync(f => f.FlightNumber == flightNumber) ?? throw new FlightNotFoundException();
    }
}
