using RentABike.Models;

namespace RentABike.Managers;

internal sealed class CustomerManager
{
    private readonly List<Customer> _customers;

    public CustomerManager()
    {
        _customers = [];
    }

    public bool RegisterCustomer(string name, int age, bool isMember)
    {
        throw new NotImplementedException();
    }
}
