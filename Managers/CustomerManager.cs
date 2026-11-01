using RentABike.Models.Users;

namespace RentABike.Managers;

internal sealed class CustomerManager
{
    private readonly List<Customer> _customers;

    public CustomerManager()
    {
        _customers = [];
    }

    public void RegisterCustomer()
    {
        string? name;
        int age;
        bool isMember;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Name");
            Console.Write("-> ");
            name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))    
            {
                Console.WriteLine();
                Console.WriteLine($"{nameof(name)} cannot be empty.");
                Thread.Sleep(1000);
                continue;
            }

            break;
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Age");
            Console.Write("-> ");
            string? ageInput = Console.ReadLine();

            if (!int.TryParse(ageInput, out int ageParsed))
            {
                Console.WriteLine();
                Console.WriteLine($"{nameof(ageInput)} was invalid.");
                Thread.Sleep(1000);
                continue;
            }

            age = ageParsed;
            break;
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Member (y/n)");
                Console.Write("-> ");

            string? isMemberInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(isMemberInput))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid input");
                Thread.Sleep(1000);
                continue;
            }

            if (!isMemberInput.ToLower().Equals("y", StringComparison.CurrentCultureIgnoreCase) &&
                !isMemberInput.ToLower().Equals("n", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid input");
                Thread.Sleep(1000);
                continue;
            }

            isMember = isMemberInput.ToLower().Equals("y", StringComparison.CurrentCultureIgnoreCase);
            break;
        }

        _customers.Add(new Customer(name, age, isMember));

        Console.WriteLine();
        Console.WriteLine($"{name} is now registered.");
        Thread.Sleep(1000);
    }
}
