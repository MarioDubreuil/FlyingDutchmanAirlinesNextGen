using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines_Tests.Stubs;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer;

[TestClass]
public class CustomerRepositoryTests
{
    private FlyingDutchmanAirlinesContext _context = null!;
    private CustomerRepository _repository = null!;

    [TestInitialize]
    public async Task TestInitialize()
    {
        DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions =
            new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                .UseInMemoryDatabase("FlyingDutchman")
                .Options;
        _context = new FlyingDutchmanAirlinesContext_Stub(dbContextOptions);

        var testCustomer = new Customer("Linus Torvalds");
        _context.Customers.Add(testCustomer);
        await _context.SaveChangesAsync();

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
        CustomerRepository repository = new(null!);
        Assert.IsNotNull(repository);
        bool result = await repository.CreateCustomer("Elvis1");
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task CreateCustomer_Failure_NameIsNull()
    {
        bool result = await _repository.CreateCustomer(null!);
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
        const string name = "Linus Torvalds";
        Customer customer = await _repository.GetCustomerByName(name);
        Assert.IsNotNull(customer);
        Assert.AreEqual(name, customer.Name);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("!")]
    [DataRow("@")]
    [DataRow("#")]
    [DataRow("$")]
    [DataRow("%")]
    [DataRow("&")]
    [DataRow("*")]
    [ExpectedException(typeof(CustomerNotFoundException))]
    public async Task GetCustomerByName_Failure_InvalidName(string name)
    {
        await _repository.GetCustomerByName(name);
    }

    [TestMethod]
    [ExpectedException(typeof(CustomerNotFoundException))]
    public async Task GetCustomerByName_Failure_NullName()
    {
        await _repository.GetCustomerByName(null!);
    }

    [TestMethod]
    [DataRow("abc")]
    [DataRow("Elvis1")]
    [ExpectedException(typeof(CustomerNotFoundException))]
    public async Task GetCustomerByName_Failure_NameNotFound(string name)
    {
        await _repository.GetCustomerByName(name);
    }
}