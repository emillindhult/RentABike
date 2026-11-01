using RentABike.Models.Bike;
using RentABike.Models.Users;

namespace RentABike;

internal static class SeedData
{
    public static List<BaseBike> SeedBikes(List<BaseBike> list)
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

    public static List<Customer> SeedCostumers(List<Customer> list)
    {
        list.Add(new Customer("Emil", 30, true));
        list.Add(new Customer("Niklas", 17, false));
        list.Add(new Customer("Lars", 14, false));
        list.Add(new Customer("Carl", 22, false));
        list.Add(new Customer("Martin", 18, true));

        return list;
    }
}
