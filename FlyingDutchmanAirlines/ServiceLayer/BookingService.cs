using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;

namespace FlyingDutchmanAirlines.ServiceLayer;

public class BookingService
{
    private readonly BookingRepository _bookingRepository = null!;
    private readonly CustomerRepository _customerRepository = null!;
    private readonly FlightRepository _flightRepository = null!;

    public BookingService()
    {
    }

    public BookingService(BookingRepository bookingRepository, CustomerRepository customerRepository, FlightRepository flightRepository)
    {
        _bookingRepository = bookingRepository;
        _customerRepository = customerRepository;
        _flightRepository = flightRepository;
    }

    public virtual async Task<(bool, Exception)> CreateBooking(string customerName, int flightNumber)
    {
        if (string.IsNullOrEmpty(customerName) || !flightNumber.IsPositive())
        {
            return (false, new ArgumentException());
        }
        try
        {
            if (! await FlightExistsInDatabase(flightNumber))
            {
                throw new CouldNotAddBookingToDatabaseException();
            }
            Customer customer;
            try
            {
                customer = await _customerRepository.GetCustomerByName(customerName);
            }
            catch (CustomerNotFoundException)
            {
                await _customerRepository.CreateCustomer(customerName);
                return await CreateBooking(customerName, flightNumber);
            }
            await _bookingRepository.CreateBooking(customer.CustomerId, flightNumber);
            return (true, null!);
        }
        catch (Exception exception)
        {

            return (false, exception);
        }
    }

    private async Task<bool> FlightExistsInDatabase(int flightNumber)
    {
        try
        {
            _ = await _flightRepository.GetFlightByFlightNumber(flightNumber);
        }
        catch (FileNotFoundException)
        {
            return false;
        }
        return true;
    }
}
