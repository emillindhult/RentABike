using RentABike.Models;

namespace RentABike;

internal static class SeedData
{
    public static List<BaseBike> Seed(List<BaseBike> list)
    {
        list.Add(new Bike(40, true));
        list.Add(new Bike(40, true));
        list.Add(new ElectricBike(80, 70, true));
        list.Add(new ElectricBike(25, 70, true));

        return list;
    }
}
