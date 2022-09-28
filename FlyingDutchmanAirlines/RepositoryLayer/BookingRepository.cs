﻿using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class BookingRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

	public BookingRepository(FlyingDutchmanAirlinesContext context)
	{
		_context = context;
	}

	public async Task CreateBooking(int customerId, int flightNumber)
	{
		if (customerId < 0 || flightNumber < 0)
		{
			Console.WriteLine($"Argument Exception in CraeteBooking! Customerid = {customerId}, flightNumber = {flightNumber}");
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
