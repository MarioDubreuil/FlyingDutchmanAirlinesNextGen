using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines_Tests.Stubs;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests;

[TestClass]
public class BookingRepositoryTests
{
    private FlyingDutchmanAirlinesContext _context = null!;
    private BookingRepository _repository = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions =
            new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                .UseInMemoryDatabase("FlyingDutchman")
                .Options;
        _context = new FlyingDutchmanAirlinesContext_Stub(dbContextOptions);

        _repository = new BookingRepository(_context);
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    [DataRow(1, 0)]
    [DataRow(1, 1)]
    [DataRow(1, 2)]
    public async Task CreateBooking_Success(int customerId, int flightNumber)
    {
        await _repository.CreateBooking(customerId, flightNumber);
        var booking = _context.Bookings.First();
        Assert.IsNotNull(booking);
        Assert.AreEqual(customerId, booking.CustomerId);
        Assert.AreEqual(flightNumber, booking.FlightNumber);
    }

    [TestMethod]
    [DataRow(-1, 0)]
    [DataRow(0, -1)]
    [DataRow(-1, -1)]
    [ExpectedException(typeof(ArgumentException))]
    public async Task CreateBooking_Failure_InvalidInputs(int customerId, int flightNumber)
    {
        await _repository.CreateBooking(customerId, flightNumber);
    }

    [TestMethod]
    [DataRow(0, 0)]
    [DataRow(0, 1)]
    [ExpectedException(typeof(CouldNotAddBookingToDatabaseException))]
    public async Task CreateBooking_Failure_DatabaseError(int customerId, int flightNumber)
    {
        await _repository.CreateBooking(customerId, flightNumber);
    }
}
