using FlyingDutchmanAirlines.DatabaseLayer;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class BookingRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

	public BookingRepository(FlyingDutchmanAirlinesContext context)
	{
		_context = context;
	}
}
