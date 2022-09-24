using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests;

[TestClass]
public class CustomerRepositoryTests
{
    private FlyingDutchmanAirlinesContext _context;

    [TestInitialize]
    public void TestInitialize()
    {
        DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions =
            new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                .UseInMemoryDatabase("FlyingDutchman")
                .Options;
        _context = new FlyingDutchmanAirlinesContext(dbContextOptions);
    }

    [TestMethod]
    public async Task CreateCustomer_Success()
    {
        CustomerRepository repository = new(_context);
        Assert.IsNotNull(repository);
        bool result = await repository.CreateCustomer("Elvis1");
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task CreateCustomer_Failure_DatabaseAccessError()
    {
        CustomerRepository repository = new(null);
        Assert.IsNotNull(repository);
        bool result = await repository.CreateCustomer("Elvis1");
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task CreateCustomer_Failure_NameIsNull()
    {
        CustomerRepository repository = new(_context);
        Assert.IsNotNull(repository);
        bool result = await repository.CreateCustomer(null);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task CreateCustomer_Failure_NameIsEmpty()
    {
        CustomerRepository repository = new(_context);
        Assert.IsNotNull(repository);
        bool result = await repository.CreateCustomer("");
        Assert.IsFalse(result);
    }

    [TestMethod]
    [DataRow('!')]
    [DataRow('@')]
    [DataRow('#')]
    [DataRow('$')]
    [DataRow('%')]
    [DataRow('&')]
    [DataRow('*')]
    public async Task CreateCustomer_Failure_NameContainsInvalidCharacters(char invalidCharacter)
    {
        CustomerRepository repository = new(_context);
        Assert.IsNotNull(repository);
        bool result = await repository.CreateCustomer("Elvis2" + invalidCharacter);
        Assert.IsFalse(result);
    }
}