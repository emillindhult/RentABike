namespace RentABike.Models.Bike;

internal sealed class Bike : BaseBike
{
    public Bike(decimal pricePerHour, bool isFree) : base(pricePerHour, isFree)
    {
    }
}
