using RentABike.Models;

namespace RentABike.Managers;

internal sealed class BikeManager
{
    private readonly List<BaseBike> _bikes = [];

    public BikeManager()
    {
        _bikes = SeedData.Seed(_bikes);
    }

    public void ListBikes()
    {
        Console.Clear();
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

        Console.WriteLine();
        Console.Write("Press any key to continue...");
        Console.ReadKey();
    }

    public bool RentBike()
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
}
