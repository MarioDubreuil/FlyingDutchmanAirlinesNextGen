using FlyingDutchmanAirlines.ControllerLayer;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.Views;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace FlyingDutchmanAirlines_Tests.ControllerLayer;

[TestClass]
public class FlightControllerTests
{
    [TestMethod]
    public async Task GetFlights_Success()
    {
        var mockFlightService = new Mock<FlightService>();

        var flights = new List<FlightView>
        {
            new FlightView("1932", ("Groningen", "GRQ"), ("Phoenix", "PHX")),
            new FlightView("841", ("New York City", "JFK"), ("London", "LHR"))
        };

        mockFlightService
            .Setup(r => r.GetFlights())
            .Returns(FlightViewAsyncGenerator(flights));

        var controller = new FlightController(mockFlightService.Object);

        var response = await controller.GetFlights() as ObjectResult;

        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);

        var content = response.Value as Queue<FlightView>;

        Assert.IsNotNull(content);
        Assert.IsTrue(flights.All(f => content.Contains(f)));
    }

    [TestMethod]
    public async Task GetFlights_Failure_FlightNotFoundExeption_404()
    {
        var mockFlightService = new Mock<FlightService>();

        mockFlightService
            .Setup(r => r.GetFlights())
            .Throws(new FlightNotFoundException());

        var controller = new FlightController(mockFlightService.Object);

        var response = await controller.GetFlights() as ObjectResult;

        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.NotFound, response.StatusCode);

        var content = response.Value;

        Assert.IsNotNull(content);
        Assert.AreEqual("No flights were found in the database", content);
    }

    [TestMethod]
    public async Task GetFlights_Failure_ArgumentExeption_500()
    {
        var mockFlightService = new Mock<FlightService>();

        mockFlightService
            .Setup(r => r.GetFlights())
            .Throws(new ArgumentException());

        var controller = new FlightController(mockFlightService.Object);

        var response = await controller.GetFlights() as ObjectResult;

        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.InternalServerError, response.StatusCode);

        var content = response.Value;

        Assert.IsNotNull(content);
        Assert.AreEqual("An error occurred", content);
    }

    [TestMethod]
    public async Task GetFlightByFlightNumber_Success()
    {
        var mockFlightService = new Mock<FlightService>();

        var flight = new FlightView("841", ("New York City", "JFK"), ("London", "LHR"));

        mockFlightService
            .Setup(r => r.GetFlightByFlightNumber(841))
            .ReturnsAsync(flight);

        var controller = new FlightController(mockFlightService.Object);

        var response = await controller.GetFlightByFlightNumber(841) as ObjectResult;

        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);

        var content = response.Value as FlightView;

        Assert.IsNotNull(content);
        Assert.AreEqual(flight, content);
    }

    [TestMethod]
    public async Task GetFlightByFlightNumber_Failure_FlightNotFoundExeption_404()
    {
        var mockFlightService = new Mock<FlightService>();

        mockFlightService
            .Setup(r => r.GetFlightByFlightNumber(841))
            .Throws(new FlightNotFoundException());

        var controller = new FlightController(mockFlightService.Object);

        var response = await controller.GetFlightByFlightNumber(841) as ObjectResult;

        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.NotFound, response.StatusCode);

        var content = response.Value;

        Assert.IsNotNull(content);
        Assert.AreEqual("Flight was not found in the database", content);
    }

    [TestMethod]
    public async Task GetFlightByFlightNumber_Failure_ArgumentExeption_400()
    {
        var mockFlightService = new Mock<FlightService>();

        mockFlightService
            .Setup(r => r.GetFlightByFlightNumber(-1))
            .Throws(new ArgumentException());

        var controller = new FlightController(mockFlightService.Object);

        var response = await controller.GetFlightByFlightNumber(-1) as ObjectResult;

        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.BadRequest, response.StatusCode);

        var content = response.Value;

        Assert.IsNotNull(content);
        Assert.AreEqual("Bad request", content);
    }

    [TestMethod]
    public async Task GetFlightByFlightNumber_Failure_ArgumentExeption_500()
    {
        var mockFlightService = new Mock<FlightService>();

        mockFlightService
            .Setup(r => r.GetFlightByFlightNumber(841))
            .Throws(new IndexOutOfRangeException());

        var controller = new FlightController(mockFlightService.Object);

        var response = await controller.GetFlightByFlightNumber(841) as ObjectResult;

        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.InternalServerError, response.StatusCode);

        var content = response.Value;

        Assert.IsNotNull(content);
        Assert.AreEqual("An error occurred", content);
    }

    private async IAsyncEnumerable<FlightView> FlightViewAsyncGenerator(IEnumerable<FlightView> flights)
    {
        foreach (var flight in flights)
        {
            yield return flight;
        }
    }
}
