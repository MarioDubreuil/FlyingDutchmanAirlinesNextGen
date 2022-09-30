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
    public async Task<(bool, Exception)> CreateBooking(int customerId, int flightNumber)
    {
        return (true, null!);
    }
}
