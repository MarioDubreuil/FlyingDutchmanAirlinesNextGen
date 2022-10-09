using FlyingDutchmanAirlines.ControllerLayer;
using FlyingDutchmanAirlines.ControllerLayer.JsonData;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace FlyingDutchmanAirlines_Tests.ControllerLayer;

[TestClass]
public class BookingControllerTests
{
    [TestMethod]
    public async Task CreateBooking_Failure_FlightNumberInvalid()
    {
        var mockService = new Mock<BookingService>();

        var controller = new BookingController(mockService.Object);

        var body = new BookingData();
        var flightNumber = -1;

        var response = await controller.CreateBooking(body, flightNumber) as ObjectResult;

        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task CreateBooking_Failure_NameInvalid()
    {
        var mockService = new Mock<BookingService>();

        var controller = new BookingController(mockService.Object);

        var body = new BookingData();
        var flightNumber = 1;

        var response = await controller.CreateBooking(body, flightNumber) as ObjectResult;

        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.BadRequest, response.StatusCode);
    }

    //[TestMethod]
    //public async Task CreateBooking_Success()
    //{
    //    var mockService = new Mock<BookingService>();

    //    var body = new BookingData
    //    {
    //        FirstName = "Peter",
    //        LastName = "Pan"
    //    };
    //    var name = $"{body.FirstName} {body.LastName}";
    //    var flightNumber = 58;

    //    mockService
    //        .Setup(s => s.CreateBooking(name, flightNumber))
    //        .ReturnsAsync((true, null!));

    //    var controller = new BookingController(mockService.Object);

    //    var response = await controller.CreateBooking(body, flightNumber) as ObjectResult;

    //    Assert.IsNotNull(response);
    //    Assert.AreEqual((int)HttpStatusCode.BadRequest, response.StatusCode);
    //}

}
