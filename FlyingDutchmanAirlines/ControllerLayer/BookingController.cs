using FlyingDutchmanAirlines.ControllerLayer.JsonData;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
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
		if (!flightNumber.IsPositive())
		{
			return StatusCode((int)HttpStatusCode.BadRequest, "Flight Number query is not a positive integer");
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
		return
			exception is CouldNotAddBookingToDatabaseException
			? StatusCode((int)HttpStatusCode.NotFound)
			: StatusCode((int)HttpStatusCode.InternalServerError);
	}
}
