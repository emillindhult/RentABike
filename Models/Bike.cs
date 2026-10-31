namespace RentABike.Models;

internal sealed class Bike : BaseBike
{
    public Bike(decimal pricePerHour, bool isFree) : base(pricePerHour, isFree)
    {
    }
}
