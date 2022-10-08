using FlyingDutchmanAirlines.ControllerLayer.JsonData;

namespace FlyingDutchmanAirlines_Tests.ControllerLayer.JsonData;

[TestClass]
public class BookingDataTests
{
    [TestMethod]
    public void BookingData_Success()
    {
        string firstName = "Marina";
        string lastName = "Michaels";
        var bookingData = new BookingData
        {
            FirstName = firstName,
            LastName = lastName
        };
        Assert.IsNotNull(bookingData);
        Assert.AreEqual(firstName, bookingData.FirstName);
        Assert.AreEqual(lastName, bookingData.LastName);
    }

    [TestMethod]
    [DataRow("", "")]
    [DataRow("Marina", "")]
    [DataRow("", "Michales")]
    [ExpectedException(typeof(InvalidOperationException))]
    public void BookingData_Failure_EmptyName(string firstName, string lastName)
    {
        var bookingData = new BookingData
        {
            FirstName = firstName,
            LastName = lastName
        };
    }

    [TestMethod]
    [DataRow(null, null)]
    [DataRow("Marina", null)]
    [DataRow(null, "Michales")]
    [ExpectedException(typeof(InvalidOperationException))]
    public void BookingData_Failure_NullName(string firstName, string lastName)
    {
        var bookingData = new BookingData
        {
            FirstName = firstName,
            LastName = lastName
        };
    }
}
