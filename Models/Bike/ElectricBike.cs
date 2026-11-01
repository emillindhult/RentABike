namespace RentABike.Models.Bike;

internal sealed class ElectricBike : BaseBike
{
    private readonly object _batteryLock = new();
    private int _battery;

    public const int MaxBattery = 100;

    public int Battery
    {
        get
        {
            lock (_batteryLock)
            {
                return _battery;
            }
        }
    }

    public ElectricBike(int battery, decimal pricePerHour, bool isFree) 
        : base(pricePerHour, isFree)
    {
        _battery = battery;
    }

    public void DrainBattery(int hours)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(hours);

        lock (_batteryLock)
        {
            _battery = Math.Max(0, _battery - (int)Math.Min(_battery, 10L * hours));
        }
    }

    public bool ChargeBatteryByOne()
    {
        lock (_batteryLock)
        {
            if (_battery < MaxBattery)
            {
                _battery++;
            }

            return _battery >= MaxBattery;
        }
    }
}
