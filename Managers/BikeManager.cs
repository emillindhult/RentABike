using RentABike.Models.Bike;
using RentABike.Models.Users;

namespace RentABike.Managers;

internal sealed class BikeManager
{
    private readonly List<BaseBike> _bikes = [];
    private readonly List<RentedBike> _rentedBikes = [];
    private readonly CustomerManager _customerManager;

    public BikeManager(CustomerManager customerManager)
    {
        _bikes = SeedData.SeedBikes(_bikes);
        _customerManager = customerManager;
    }

    public void ListBikes()
    {
        string? userInput;

        while (true)
        {
            Console.Clear();
            RenderBikes();
            Console.WriteLine();
            Console.WriteLine("0. Back");
            Console.WriteLine();
            Console.Write("-> ");

            userInput = Console.ReadLine();

            if (userInput == "0")
            {
                return;
            }

            RenderSelectedBike(userInput);
        }
    }

    private void RenderSelectedBike(string? input)
    {
        if (!int.TryParse(input, out int index) || index < 1 || index > _bikes.Count)
        {
            return;
        }

        BaseBike bike = _bikes[index - 1];
        string type = bike is ElectricBike ? "Electric Bike" : "Bike";
        string battery = bike is ElectricBike electricBike
            ? $"{electricBike.Battery}%"
            : "N/A";

        Console.Clear();
        Console.WriteLine("+----------------------------------------+");
        Console.WriteLine($"| Bike #{index} Details".PadRight(41) + "|");
        Console.WriteLine("+----------------------------------------+");
        Console.WriteLine($"| Type       : {type,-26}|");
        Console.WriteLine($"| Price/hour : {bike.PricePerHour + ":-",-26}|");
        Console.WriteLine($"| Status     : {(bike.IsFree ? "Available" : "Rented"),-26}|");
        Console.WriteLine($"| Battery    : {battery,-26}|");
        Console.WriteLine("+----------------------------------------+");
        Console.WriteLine();
        Console.Write("Press any key to return to the bike list...");
        Console.ReadKey(true);
    }

    private void RenderBikes()
    {
        Console.WriteLine(new string('-', 55));
        Console.WriteLine("{0,-3} {1,-14} {2,-12} {3,-12} {4,-10}",
            "No.", "Type", "Price/hour", "Status", "Battery");
        Console.WriteLine(new string('-', 55));

        for (int i = 0; i < _bikes.Count; i++)
        {
            BaseBike bike = _bikes[i];
            string type = bike is ElectricBike ? "Electric Bike" : "Bike";
            string battery = bike is ElectricBike electricBike
                ? $"{electricBike.Battery}%"
                : "N/A";

            Console.WriteLine("{0,-3} {1,-14} {2,-12} {3,-12} {4,-10}",
                i + 1,
                type,
                bike.PricePerHour + ":-",
                bike.IsFree ? "Available" : "Rented",
                battery);
        }
    }

    public void RentBike()
    {
        string? userInput;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Select a bike to rent out");
            RenderBikes();
            Console.WriteLine();
            Console.WriteLine("0. Back");
            Console.WriteLine();
            Console.Write("-> ");

            userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid input");
                Thread.Sleep(1000);
                continue;
            }

            if (userInput == "0")
            {
                return;
            }

            if (!int.TryParse(userInput, out int index) || index < 1 || index > _bikes.Count)
            {
                Console.WriteLine();
                Console.WriteLine("Invalid input");
                Thread.Sleep(1000);
                continue;
            }

            var bike = _bikes[index - 1];

            if (!bike.IsFree)
            {
                Console.WriteLine();
                Console.WriteLine("The bike is not available.");
                Thread.Sleep(1000);
                return;
            }

            if (bike is Bike ordinaryBike)
            {
                RentOrdinaryBike(ordinaryBike);
            }
            else if (bike is ElectricBike electricBike)
            {
                RentElectricBike(electricBike);
            }
        }
    }

    private void RentElectricBike(ElectricBike bike)
    {
        if (bike.Battery <= 30)
        {
            Console.WriteLine();
            Console.WriteLine("The bike needs to charge.");
            Thread.Sleep(1000);
            return;
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Choose a customer");
            RenderCustomers();
            Console.WriteLine();
            Console.Write("0. Back");
            Console.WriteLine();
            Console.Write("-> ");

            string? userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid input");
                Thread.Sleep(1000);
                continue;
            }

            if (userInput == "0")
            {
                return;
            }

            if (!int.TryParse(userInput, out int index) || index < 1 || index > _customerManager.Customers.Count)
            {
                Console.WriteLine();
                Console.WriteLine("Invalid input");
                Thread.Sleep(1000);
                continue;
            }

            var customer = _customerManager.Customers[index - 1];

            if (customer.Age < 18)
            {
                Console.WriteLine();
                Console.WriteLine("Customer need to be 18 or older.");
                Thread.Sleep(1000);
                continue;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("How long to rent out");
                Console.Write("-> ");

                string? lengthInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(lengthInput))
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid input");
                    Thread.Sleep(1000);
                    continue;
                }

                if (!int.TryParse(lengthInput, out int length))
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid input");
                    Thread.Sleep(1000);
                    continue;
                }

                if (length * 10 > bike.Battery)
                {
                    Console.WriteLine();
                    Console.WriteLine("The bike needs to charge before renting out that long.");
                    Thread.Sleep(1000);
                    continue;
                }

                _rentedBikes.Add(new RentedBike(customer.Id, bike.Id, BikeType.ElectricBike, length));
                bike.SetIsFree(false);

                Console.WriteLine();
                Console.WriteLine("Bike successfully rented out.");
                Thread.Sleep(1000);
                return;
            }
        }
    }

    private void RentOrdinaryBike(Bike bike)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Choose a customer");
            RenderCustomers();
            Console.WriteLine();
            Console.Write("0. Back");
            Console.WriteLine();
            Console.Write("-> ");

            string? userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid input");
                Thread.Sleep(1000);
                continue;
            }

            if (userInput == "0")
            {
                return;
            }

            if (!int.TryParse(userInput, out int index) || index < 1 || index > _customerManager.Customers.Count)
            {
                Console.WriteLine();
                Console.WriteLine("Invalid input");
                Thread.Sleep(1000);
                continue;
            }

            var customer = _customerManager.Customers[index - 1];

            if (customer.Age < 15)
            {
                Console.WriteLine();
                Console.WriteLine("Customer need to be 15 or older.");
                Thread.Sleep(1000);
                continue;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("How long to rent out");
                Console.Write("-> ");

                string? lengthInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(lengthInput))
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid input");
                    Thread.Sleep(1000);
                    continue;
                }

                if (!int.TryParse(lengthInput, out int length))
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid input");
                    Thread.Sleep(1000);
                    continue;
                }

                _rentedBikes.Add(new RentedBike(customer.Id, bike.Id, BikeType.Bike, length));
                bike.SetIsFree(false);

                Console.WriteLine();
                Console.WriteLine("Bike successfully rented out.");
                Thread.Sleep(1000);
                return;
            }
        }
    }

    public void ReturnBike()
    {
        throw new NotImplementedException();
    }

    public void ChargeBike()
    {
        throw new NotImplementedException();
    }

    private void RenderCustomers()
    {
        IReadOnlyList<Customer> customers = _customerManager.Customers;

        Console.WriteLine(new string('-', 48));
        Console.WriteLine("{0,-3} {1,-24} {2,-6} {3,-10}",
            "No.", "Name", "Age", "Member");
        Console.WriteLine(new string('-', 48));

        for (int i = 0; i < customers.Count; i++)
        {
            Customer customer = customers[i];
            Console.WriteLine("{0,-3} {1,-24} {2,-6} {3,-10}",
                i + 1,
                customer.Name,
                customer.Age,
                customer.IsMember ? "Yes" : "No");
        }
    }
}
