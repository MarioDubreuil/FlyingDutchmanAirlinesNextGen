using FlyingDutchmanAirlines.ControllerLayer.JsonData;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Net;

namespace FlyingDutchmanAirlines.ControllerLayer;

[ApiController]
[Route("[controller]")]
public class BookingController : ControllerBase
{
    private readonly BookingService _service;

	public BookingController(BookingService service)
	{
		_service = service;
	}

	[HttpPost("{flightNumber")]
	public async Task<IActionResult> CreateBooking([FromBody] BookingData body, int flightNumber)
	{
		if (!flightNumber.IsPositive() || String.IsNullOrEmpty(body.FirstName) || String.IsNullOrEmpty(body.LastName))

        {
			return StatusCode((int)HttpStatusCode.BadRequest, "Flight Number query is not a positive integer or First or Last Name is null or empty");
		}
		if (!ModelState.IsValid)
		{
			return StatusCode((int)HttpStatusCode.BadRequest, ModelState.Root.Errors.First().ErrorMessage);
		}
		var name = $"{body.FirstName} {body.LastName}";
		(var result, var exception) = await _service.CreateBooking(name, flightNumber);
		if (result)
		{
			return StatusCode((int)HttpStatusCode.Created);
		}
		if (exception is CouldNotAddBookingToDatabaseException)
		{
			return StatusCode((int)HttpStatusCode.NotFound);

        }
		return StatusCode((int)HttpStatusCode.InternalServerError);
	}
}
