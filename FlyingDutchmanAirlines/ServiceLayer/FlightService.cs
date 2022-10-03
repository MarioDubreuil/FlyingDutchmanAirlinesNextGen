using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.Views;

namespace FlyingDutchmanAirlines.ServiceLayer;

public class FlightService
{
    private readonly FlightRepository _flightRepository;
	private readonly AirportRepository _airportRepository;

	public FlightService(FlightRepository flightRepository, AirportRepository airportRepository)
	{
		_flightRepository = flightRepository;
		_airportRepository = airportRepository;
	}

	public async IAsyncEnumerable<FlightView> GetFlights()
	{
		var flights = _flightRepository.GetFlights();
		foreach (var flight in flights)
		{
            var originAirport = await _airportRepository.GetAirportById(flight.Origin);
            var destinationAirport = await _airportRepository.GetAirportById(flight.Destination);
            yield return new FlightView(flight.FlightNumber.ToString(), (originAirport.City, originAirport.Iata), (destinationAirport.City, destinationAirport.Iata));
		}
	}
}
