using FlyingDutchmanAirlines;
using FlyingDutchmanAirlines.RepositoryLayer;

namespace FlyingDutchmanAirlines_Tests;

[TestClass]
public class CustomerRepositoryTests
{
    [TestMethod]
    public void CreateCustomer_Success()
    {
        CustomerRepository repository = new();
        Assert.IsNotNull(repository);
    }
}