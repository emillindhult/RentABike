namespace RentABike.Models;

internal abstract class BaseBike
{
    public Guid Id { get; }
    public decimal PricePerHour { get; }
    public bool IsFree { get; }

    public BaseBike(decimal pricePerHour, bool isFree)
    {
        Id = Guid.NewGuid();
        PricePerHour = pricePerHour;
        IsFree = isFree;
    }

    public decimal CalculatePrice(int hours)
    {
        throw new NotImplementedException();
    }
}
