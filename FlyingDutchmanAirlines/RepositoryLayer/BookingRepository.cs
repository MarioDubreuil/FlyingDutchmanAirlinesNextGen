using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class BookingRepository
{
    private readonly FlyingDutchmanAirlinesContext _context = null!;

	public BookingRepository()
	{
	}

	public BookingRepository(FlyingDutchmanAirlinesContext context)
	{
		_context = context;
	}

	public virtual async Task CreateBooking(int customerId, int flightNumber)
	{
		if (!customerId.IsPositive() || !flightNumber.IsPositive())
		{
			Console.WriteLine($"Argument Exception in CreateBooking! Customerid = {customerId}, flightNumber = {flightNumber}");
			throw new ArgumentException("Invalid arguments provided");
		}
		var newBooking = new Booking()
		{
			CustomerId = customerId,
			FlightNumber = flightNumber
		};

		try
		{
			_context.Bookings.Add(newBooking);
			await _context.SaveChangesAsync();
		}
		catch (Exception exception)
		{
			Console.WriteLine($"Exception during database query: {exception.Message}");
			throw new CouldNotAddBookingToDatabaseException();
		}
    }
}
