using FlyingDutchmanAirlines.RepositoryLayer;

namespace FlyingDutchmanAirlines.ServiceLayer;

public class FlightService
{
    private readonly FlightRepository _flightRepository;

	public FlightService(FlightRepository flightRepository)
	{
		_flightRepository = flightRepository;
	}
}
