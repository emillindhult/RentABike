namespace RentABike.Models.Bike;

internal sealed class ElectricBike : BaseBike
{
    public const int MaxBattery = 100;

    public int Battery { get; private set; }

    public ElectricBike(int battery, decimal pricePerHour, bool isFree) 
        : base(pricePerHour, isFree)
    {
        Battery = battery;
    }

    public void DrainBattery(int hours)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(hours);
        Battery = Math.Max(0, Battery - (int)Math.Min(Battery, 10L * hours));
    }

    public void ChargeBattery()
    {
        Battery = MaxBattery;
    }
}
