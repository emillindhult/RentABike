namespace RentABike.Models.Bike;

internal sealed class ElectricBike : BaseBike
{
    public int Battery { get; }

    public ElectricBike(int battery, decimal pricePerHour, bool isFree) 
        : base(pricePerHour, isFree)
    {
        Battery = battery;
    }
}
