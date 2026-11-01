using RentABike.Models.Bike;

namespace RentABike;

internal static class SeedData
{
    public static List<BaseBike> Seed(List<BaseBike> list)
    {
        list.Add(new Bike(40, true));
        list.Add(new Bike(40, true));
        list.Add(new ElectricBike(80, 70, true));
        list.Add(new ElectricBike(25, 70, true));
        list.Add(new Bike(40, false));
        list.Add(new Bike(40, false));
        list.Add(new ElectricBike(50, 70, false));
        list.Add(new Bike(40, true));
        list.Add(new ElectricBike(12, 70, true));
        list.Add(new ElectricBike(68, 70, false));
        list.Add(new Bike(40, false));
        //FileHandler.AddRangeItems(list);

        return list;
    }
}
