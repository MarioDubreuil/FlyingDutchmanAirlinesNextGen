namespace FlyingDutchmanAirlines.DatabaseLayer.Models;

public class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; } = null!;
    public virtual ICollection<Booking> Bookings { get; set; }

    public Customer()
    {
        Bookings = new HashSet<Booking>();
    }
}
