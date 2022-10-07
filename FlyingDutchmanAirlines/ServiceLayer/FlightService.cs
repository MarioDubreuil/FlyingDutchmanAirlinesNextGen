using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.Views;

namespace FlyingDutchmanAirlines.ServiceLayer;

public class FlightService
{
    private readonly FlightRepository _flightRepository = null!;
	private readonly AirportRepository _airportRepository = null!;

    public FlightService()
    {
    }

	public FlightService(FlightRepository flightRepository, AirportRepository airportRepository)
	{
		_flightRepository = flightRepository;
		_airportRepository = airportRepository;
	}

    public virtual async IAsyncEnumerable<FlightView> GetFlights()
    {
        var flights = _flightRepository.GetFlights();
        foreach (var flight in flights)
        {
            Airport originAirport;
            Airport destinationAirport;
            try
            {
                originAirport = await _airportRepository.GetAirportById(flight.Origin);
                destinationAirport = await _airportRepository.GetAirportById(flight.Destination);
            }
            catch (AirportNotFoundException)
            {
                throw new AirportNotFoundException();
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }
            yield return new FlightView(flight.FlightNumber.ToString(), (originAirport.City, originAirport.Iata), (destinationAirport.City, destinationAirport.Iata));
        }
    }

    public virtual async Task<FlightView> GetFlightByFlightNumber(int flightNumber)
    {
        try
        {
            var flight = await _flightRepository.GetFlightByFlightNumber(flightNumber);
            var originAirport = await _airportRepository.GetAirportById(flight.Origin);
            var destinationAirport = await _airportRepository.GetAirportById(flight.Destination);
            return new FlightView(flight.FlightNumber.ToString(), (originAirport.City, originAirport.Iata), (destinationAirport.City, destinationAirport.Iata));
        }
        catch (FlightNotFoundException)
        {
            throw new FlightNotFoundException();
        }
        catch (Exception)
        {
            throw new ArgumentException();
        }
    }
}
