using RentABike.Models.Bike;

namespace RentABike.Managers;

internal sealed class BikeManager
{
    private readonly List<BaseBike> _bikes = [];

    public BikeManager()
    {
        _bikes = FileHandler.ReadItems();
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

            if (!int.TryParse(userInput, out int index))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid input");
                Thread.Sleep(1000);
                continue;
            }

            var bike = _bikes[index - 1];
            var battery = bike is ElectricBike electricBike ? electricBike.Battery : -1;

            if (battery < 0)
            {
                RentOrdinaryBike();
            }

            if (battery <= 25)
            {
                Console.WriteLine();
                Console.WriteLine("The bike needs charging.");
                Thread.Sleep(1000);
                continue;
            }

        }
    }

    private void RentOrdinaryBike()
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }
}
