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

	public async Task<Flight> GetFlightByFlightNumber(int flightNumber, int origin, int destination)
	{
        if (!flightNumber.IsPositive() || !origin.IsPositive() || !destination.IsPositive())
        {
            Console.WriteLine($"Argument Exception in GetFlightByFlightNumber! FlightNumber = {flightNumber}, Origin = {origin}, Destination = {destination}");
            throw new ArgumentException("Invalid arguments provided");
        }
        return await _context.Flights.FirstOrDefaultAsync(f => f.FlightNumber == flightNumber && f.Origin == origin && f.Destination == destination) ?? throw new FlightNotFoundException();
    }
}
