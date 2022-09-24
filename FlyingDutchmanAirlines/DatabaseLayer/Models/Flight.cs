namespace FlyingDutchmanAirlines.DatabaseLayer.Models;

public class Flight
{
    public int FlightNumber { get; set; }
    public int Origin { get; set; }
    public int Destination { get; set; }
    public virtual Airport DestinationNavigation { get; set; } = null!;
    public virtual Airport OriginNavigation { get; set; } = null!;
    public virtual ICollection<Booking> Bookings { get; set; }

    public Flight()
    {
        Bookings = new HashSet<Booking>();
    }
}
