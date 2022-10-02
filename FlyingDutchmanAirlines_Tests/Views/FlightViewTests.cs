using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingDutchmanAirlines_Tests.Views;

[TestClass]
public class FlightViewTests
{
    [TestMethod]
    public void Constructor_FlightView_Success()
    {
        string flightNumber = "0";
        string originCity = "Amserdam";
        string originCode = "AMS";
        string destinationCity = "Moscow";
        string destinationCode = "SVO";

        var view = new FlightView(flightNumber, (originCity, originCode), (destinationCity, destinationCode));

        Assert.IsNotNull(view);

        Assert.AreEqual(flightNumber, view.FlightNumber);
        Assert.AreEqual(originCity, view.Origin.City);
        Assert.AreEqual(originCode, view.Origin.Code);
        Assert.AreEqual(destinationCity, view.Destination.City);
        Assert.AreEqual(destinationCode, view.Destination.Code);
    }

    [TestMethod]
    public void Constructor_FlightView_Success_FlightNumber_Null()
    {
        string originCity = "Amserdam";
        string originCode = "AMS";
        string destinationCity = "Moscow";
        string destinationCode = "SVO";

        var view = new FlightView(null!, (originCity, originCode), (destinationCity, destinationCode));

        Assert.IsNotNull(view);

        Assert.AreEqual("No flight number found", view.FlightNumber);
        Assert.AreEqual(originCity, view.Origin.City);
        Assert.AreEqual(originCode, view.Origin.Code);
        Assert.AreEqual(destinationCity, view.Destination.City);
        Assert.AreEqual(destinationCode, view.Destination.Code);
    }

    [TestMethod]
    public void Constructor_FlightView_Success_City_EmptyString()
    {
        string flightNumber = "0";
        string originCity = "";
        string originCode = "AMS";
        string destinationCity = "Moscow";
        string destinationCode = "SVO";

        var view = new FlightView(flightNumber, (originCity, originCode), (destinationCity, destinationCode));

        Assert.IsNotNull(view);

        Assert.AreEqual(flightNumber, view.FlightNumber);
        Assert.AreEqual("No city found", view.Origin.City);
        Assert.AreEqual(originCode, view.Origin.Code);
        Assert.AreEqual(destinationCity, view.Destination.City);
        Assert.AreEqual(destinationCode, view.Destination.Code);
    }

    [TestMethod]
    public void Constructor_FlightView_Success_Code_EmptyString()
    {
        string flightNumber = "0";
        string originCity = "Amserdam";
        string originCode = "AMS";
        string destinationCity = "Moscow";
        string destinationCode = "";

        var view = new FlightView(flightNumber, (originCity, originCode), (destinationCity, destinationCode));

        Assert.IsNotNull(view);

        Assert.AreEqual(flightNumber, view.FlightNumber);
        Assert.AreEqual(originCity, view.Origin.City);
        Assert.AreEqual(originCode, view.Origin.Code);
        Assert.AreEqual(destinationCity, view.Destination.City);
        Assert.AreEqual("No code found", view.Destination.Code);
    }
}
