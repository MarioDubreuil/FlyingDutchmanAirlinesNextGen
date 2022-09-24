using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class CustomerRepository
{
    public async Task<bool> CreateCustomer(string name)
    {
        if (IsInvalidCustomerName(name))
        {
            return false;
        }
        Customer newCustomer = new(name);
        using (FlyingDutchmanAirlinesContext context = new())
        {
            context.Customers.Add(newCustomer);
            await context.SaveChangesAsync();
        }
        return true;
    }

    private bool IsInvalidCustomerName(string? name)
    {
        char[] forbiddenCharacters = { '!', '@', '#', '$', '%', '&', '*' };
        return string.IsNullOrEmpty(name) || name.Any(x => forbiddenCharacters.Contains(x));
    }
}
