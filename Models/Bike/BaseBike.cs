namespace RentABike.Models.Bike;

internal abstract class BaseBike
{
    public Guid Id { get; }
    public decimal PricePerHour { get; }
    public bool IsFree { get; private set; }

    public BaseBike(decimal pricePerHour, bool isFree)
    {
        Id = Guid.NewGuid();
        PricePerHour = pricePerHour;
        IsFree = isFree;
    }

    public decimal CalculatePrice(int hours)
    {
        return PricePerHour * hours;
    }

    public void SetIsFree(bool isFree)
    {
        IsFree = isFree;
    }
}
