using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class FlightRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

	public FlightRepository(FlyingDutchmanAirlinesContext context)
	{
		_context = context;
	}

	public async Task<Flight> GetFlightByFlightNumber(int flightNumber, int origin, int destination)
	{
        if (flightNumber < 0 || origin < 0 || destination < 0)
        {
            Console.WriteLine($"Argument Exception in GetFlightByFlightNumber! FlightNumber = {flightNumber}, Origin = {origin}, Destination = {destination}");
            throw new ArgumentException("Invalid arguments provided");
        }
        return await _context.Flights.FirstOrDefaultAsync(f => f.FlightNumber == flightNumber && f.Origin == origin && f.Destination == destination) ?? throw new FlightNotFoundException();
    }
}
