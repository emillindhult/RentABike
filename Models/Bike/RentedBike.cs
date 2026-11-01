namespace RentABike.Models.Bike;

internal sealed class RentedBike
{
    public Guid Id { get; }
    public Guid BikeId { get; }
    public Guid CustomerId { get; }
    public BikeType BikeType { get; }
    public int Length { get; }

    public RentedBike(Guid customerId, Guid bikeId, BikeType bikeType, int length)
    {
        Id = Guid.NewGuid();
        BikeId = bikeId;
        CustomerId = customerId;
        BikeType = bikeType;
        Length = length;
    }
}

public enum BikeType
{
    Bike,
    ElectricBike
}
