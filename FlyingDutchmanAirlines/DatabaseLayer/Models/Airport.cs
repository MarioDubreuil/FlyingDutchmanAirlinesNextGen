namespace FlyingDutchmanAirlines.DatabaseLayer.Models;

public class Airport
{
    public int AirportId { get; set; }
    public string City { get; set; } = null!;
    public string Iata { get; set; } = null!;
    public virtual ICollection<Flight> FlightDestinationNavigations { get; set; }
    public virtual ICollection<Flight> FlightOriginNavigations { get; set; }

    public Airport()
    {
        FlightDestinationNavigations = new HashSet<Flight>();
        FlightOriginNavigations = new HashSet<Flight>();
    }
}
