using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;

namespace FlyingDutchmanAirlines.ServiceLayer;

public class BookingService
{
    private readonly BookingRepository _bookingRepository;
    private readonly CustomerRepository _customerRepository;
    private readonly FlightRepository _flightRepository;
    public BookingService(BookingRepository bookingRepository, CustomerRepository customerRepository, FlightRepository flightRepository)
    {
        _bookingRepository = bookingRepository;
        _customerRepository = customerRepository;
        _flightRepository = flightRepository;
    }
    public async Task<(bool, Exception)> CreateBooking(string customerName, int flightNumber)
    {
        try
        {
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
}
