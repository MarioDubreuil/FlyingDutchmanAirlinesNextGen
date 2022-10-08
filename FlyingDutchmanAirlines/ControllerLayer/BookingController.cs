using FlyingDutchmanAirlines.ControllerLayer.JsonData;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.AspNetCore.Mvc;

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

	//[HttpPost]
	//public async Task<IActionResult> CreateBooking([FromBody] BookingData body)
	//{

	//}
}
