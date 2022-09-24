using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests;

[TestClass]
public class CustomerRepositoryTests
{
    private FlyingDutchmanAirlinesContext _context;
    private CustomerRepository _repository;

    [TestInitialize]
    public void TestInitialize()
    {
        DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions =
            new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                .UseInMemoryDatabase("FlyingDutchman")
                .Options;
        _context = new FlyingDutchmanAirlinesContext(dbContextOptions);

        _repository = new CustomerRepository(_context);
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    public async Task CreateCustomer_Success()
    {
        bool result = await _repository.CreateCustomer("Elvis1");
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
        bool result = await _repository.CreateCustomer(null);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task CreateCustomer_Failure_NameIsEmpty()
    {
        bool result = await _repository.CreateCustomer("");
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
        bool result = await _repository.CreateCustomer("Elvis2" + invalidCharacter);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task GetCustomerByName_Success()
    {
        Customer customer = await _repository.GetCustomerName("Elvis1");
        Assert.IsNotNull(customer);
    }
}