using FlyingDutchmanAirlines.ControllerLayer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FlyingDutchmanAirlines_Tests.ControllerLayer
{
    [TestClass]
    public class FlightControllerTests
    {
        [TestMethod]
        public void GetFlights_Success()
        {
            var controller = new FlightController();
            var response = controller.GetFlights() as ObjectResult;
            Assert.IsNotNull(response);
            Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Hello, World!", response.Value);
        }
    }
}
