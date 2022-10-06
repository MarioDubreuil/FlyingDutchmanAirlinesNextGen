using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlyingDutchmanAirlines.ControllerLayer
{
    public class FlightController : ControllerBase
    {
        private readonly FlightService _service;

        public FlightController(FlightService service)
        {
            _service = service;
        }

        public IActionResult GetFlights()
        {
            return StatusCode((int)HttpStatusCode.OK, "Hello, World!");
        }
    }
}
