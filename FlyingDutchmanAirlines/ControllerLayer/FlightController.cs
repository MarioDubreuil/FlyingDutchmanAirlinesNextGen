using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.Views;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlyingDutchmanAirlines.ControllerLayer
{
    [ApiController]
    [Route("[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly FlightService _service;

        public FlightController(FlightService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetFlights()
        {
            var flights = new Queue<FlightView>();
            try
            {
                await foreach (var flight in _service.GetFlights())
                {
                    flights.Enqueue(flight);
                }
                return StatusCode((int)HttpStatusCode.OK, flights);
            }
            catch (FlightNotFoundException)
            {
                return StatusCode((int)HttpStatusCode.NotFound, "No flights were found in the database");
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred");
            }
        }

        [HttpGet("{flightNumber}")]
        public async Task<IActionResult> GetFlightByFlightNumber(int flightNumber)
        {
            try
            {
                var flight = await _service.GetFlightByFlightNumber(flightNumber);
                return StatusCode((int)HttpStatusCode.OK, flight);
            }
            catch (FlightNotFoundException)
            {
                return StatusCode((int)HttpStatusCode.NotFound, "Flight was not found in the database");
            }
            catch (ArgumentException)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Bad request");
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred");
            }
        }
    }
}
