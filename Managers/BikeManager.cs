using System.Collections.Concurrent;
using RentABike.Models.Bike;
using RentABike.Models.Users;
using System.Globalization;

namespace RentABike.Managers;

internal sealed class BikeManager
{
    private readonly List<BaseBike> _bikes = [];
    private readonly List<RentedBike> _rentedBikes = [];
    private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _chargingBikeIds = new();
    private readonly CustomerManager _customerManager;

    public BikeManager(CustomerManager customerManager)
    {
        _bikes = SeedData.SeedBikes(_bikes);
        _customerManager = customerManager;
    }

    public void ListBikes()
    {
        string? userInput;

        while (true)
        {
            Console.Clear();
            RenderBikes();
            Console.WriteLine();
            Console.WriteLine("0. Back");
            Console.WriteLine();
            Console.Write("-> ");

            userInput = Console.ReadLine();

            if (userInput == "0")
            {
                return;
            }

            RenderSelectedBike(userInput);
        }
    }

    private void RenderSelectedBike(string? input)
    {
        if (!int.TryParse(input, out int index) || index < 1 || index > _bikes.Count)
        {
            return;
        }

        BaseBike bike = _bikes[index - 1];
        string type = bike is ElectricBike ? "Electric Bike" : "Bike";
        string battery = bike is ElectricBike electricBike
            ? $"{electricBike.Battery}%"
            : "N/A";

        Console.Clear();
        Console.WriteLine("+----------------------------------------+");
        Console.WriteLine($"| Bike #{index} Details".PadRight(41) + "|");
        Console.WriteLine("+----------------------------------------+");
        Console.WriteLine($"| Type       : {type,-26}|");
        Console.WriteLine($"| Price/hour : {bike.PricePerHour + ":-",-26}|");
        Console.WriteLine($"| Status     : {(bike.IsFree ? "Available" : "Rented"),-26}|");
        Console.WriteLine($"| Battery    : {battery,-26}|");
        Console.WriteLine("+----------------------------------------+");
        Console.WriteLine();
        Console.Write("Press any key to return to the bike list...");
        Console.ReadKey(true);
    }

    private void RenderBikes()
    {
        Console.WriteLine(new string('-', 55));
        Console.WriteLine("{0,-3} {1,-14} {2,-12} {3,-12} {4,-10}",
            "No.", "Type", "Price/hour", "Status", "Battery");
        Console.WriteLine(new string('-', 55));

        for (int i = 0; i < _bikes.Count; i++)
        {
            BaseBike bike = _bikes[i];
            string type = bike is ElectricBike ? "Electric Bike" : "Bike";
            string battery = bike is ElectricBike electricBike
                ? $"{electricBike.Battery}%"
                : "N/A";

            Console.WriteLine("{0,-3} {1,-14} {2,-12} {3,-12} {4,-10}",
                i + 1,
                type,
                bike.PricePerHour + ":-",
                _chargingBikeIds.ContainsKey(bike.Id)
                    ? "Charging"
                    : bike.IsFree ? "Available" : "Rented",
                battery);
        }
    }

    private void RenderAvailableBikes()
    {
        Console.WriteLine(new string('-', 55));
        Console.WriteLine("{0,-3} {1,-14} {2,-12} {3,-12} {4,-10}",
            "No.", "Type", "Price/hour", "Status", "Battery");
        Console.WriteLine(new string('-', 55));

        var availableBikes = _bikes
            .Where(bike => bike.IsFree && !_chargingBikeIds.ContainsKey(bike.Id))
            .ToList();

        for (int i = 0; i < availableBikes.Count; i++)
        {
            BaseBike bike = availableBikes[i];
            string type = bike is ElectricBike ? "Electric Bike" : "Bike";
            string battery = bike is ElectricBike electricBike
                ? $"{electricBike.Battery}%"
                : "N/A";

            Console.WriteLine("{0,-3} {1,-14} {2,-12} {3,-12} {4,-10}",
                i + 1,
                type,
                bike.PricePerHour + ":-",
                "Available",
                battery);
        }
    }

    private void RenderRentedBikes()
    {
        Console.WriteLine(new string('-', 55));
        Console.WriteLine("{0,-3} {1,-14} {2,-12} {3,-12} {4,-10}",
            "No.", "Type", "Price/hour", "Status", "Battery");
        Console.WriteLine(new string('-', 55));

        var availableBikes = _bikes.Where(b => b.IsFree == false).ToList();

        for (int i = 0; i < availableBikes.Count; i++)
        {
            BaseBike bike = availableBikes[i];
            string type = bike is ElectricBike ? "Electric Bike" : "Bike";
            string battery = bike is ElectricBike electricBike
                ? $"{electricBike.Battery}%"
                : "N/A";

            Console.WriteLine("{0,-3} {1,-14} {2,-12} {3,-12} {4,-10}",
                i + 1,
                type,
                bike.PricePerHour + ":-",
                "Rented",
                battery);
        }
    }

    public void RentBike()
    {
        string? userInput;
        List<BaseBike> availableBikes = _bikes
            .Where(bike => bike.IsFree && !_chargingBikeIds.ContainsKey(bike.Id))
            .ToList();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Select a bike to rent out");
            RenderAvailableBikes();
            Console.WriteLine();
            Console.WriteLine("0. Back");
            Console.WriteLine();
            Console.Write("-> ");

            userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid input");
                Thread.Sleep(1000);
                continue;
            }

            if (userInput == "0")
            {
                return;
            }

            if (!int.TryParse(userInput, out int index) || index < 1 || index > availableBikes.Count)
            {
                Console.WriteLine();
                Console.WriteLine("Invalid input");
                Thread.Sleep(1000);
                continue;
            }

            var bike = availableBikes[index - 1];

            if (!bike.IsFree || _chargingBikeIds.ContainsKey(bike.Id))
            {
                Console.WriteLine();
                Console.WriteLine("The bike is not available.");
                Thread.Sleep(1000);
                return;
            }

            if (bike is Bike ordinaryBike)
            {
                RentOrdinaryBike(ordinaryBike);
            }
            else if (bike is ElectricBike electricBike)
            {
                RentElectricBike(electricBike);
            }
        }
    }

    private void RentElectricBike(ElectricBike bike)
    {
        if (bike.Battery <= 30)
        {
            Console.WriteLine();
            Console.WriteLine("The bike needs to charge.");
            Thread.Sleep(1000);
            return;
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Choose a customer");
            RenderCustomers();
            Console.WriteLine();
            Console.Write("0. Back");
            Console.WriteLine();
            Console.Write("-> ");

            string? userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid input");
                Thread.Sleep(1000);
                continue;
            }

            if (userInput == "0")
            {
                return;
            }

            if (!int.TryParse(userInput, out int index) || index < 1 || index > _customerManager.Customers.Count)
            {
                Console.WriteLine();
                Console.WriteLine("Invalid input");
                Thread.Sleep(1000);
                continue;
            }

            var customer = _customerManager.Customers[index - 1];

            if (customer.Age < 18)
            {
                Console.WriteLine();
                Console.WriteLine("Customer need to be 18 or older.");
                Thread.Sleep(1000);
                continue;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("How long to rent out");
                Console.Write("-> ");

                string? lengthInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(lengthInput))
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid input");
                    Thread.Sleep(1000);
                    continue;
                }

                if (!int.TryParse(lengthInput, out int length))
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid input");
                    Thread.Sleep(1000);
                    continue;
                }

                if (length <= 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Rental length must be greater than zero.");
                    Thread.Sleep(1000);
                    continue;
                }

                if (10L * length > bike.Battery)
                {
                    Console.WriteLine();
                    Console.WriteLine("The bike needs to charge before renting out that long.");
                    Thread.Sleep(1000);
                    continue;
                }

                _rentedBikes.Add(new RentedBike(customer.Id, bike.Id, BikeType.ElectricBike, length));
                bike.SetIsFree(false);

                Console.WriteLine();
                Console.WriteLine("Bike successfully rented out.");
                Thread.Sleep(1000);
                return;
            }
        }
    }

    private void RentOrdinaryBike(Bike bike)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Choose a customer");
            RenderCustomers();
            Console.WriteLine();
            Console.Write("0. Back");
            Console.WriteLine();
            Console.Write("-> ");

            string? userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid input");
                Thread.Sleep(1000);
                continue;
            }

            if (userInput == "0")
            {
                return;
            }

            if (!int.TryParse(userInput, out int index) || index < 1 || index > _customerManager.Customers.Count)
            {
                Console.WriteLine();
                Console.WriteLine("Invalid input");
                Thread.Sleep(1000);
                continue;
            }

            var customer = _customerManager.Customers[index - 1];

            if (customer.Age < 15)
            {
                Console.WriteLine();
                Console.WriteLine("Customer need to be 15 or older.");
                Thread.Sleep(1000);
                continue;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("How long to rent out");
                Console.Write("-> ");

                string? lengthInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(lengthInput))
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid input");
                    Thread.Sleep(1000);
                    continue;
                }

                if (!int.TryParse(lengthInput, out int length))
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid input");
                    Thread.Sleep(1000);
                    continue;
                }

                _rentedBikes.Add(new RentedBike(customer.Id, bike.Id, BikeType.Bike, length));
                bike.SetIsFree(false);

                Console.WriteLine();
                Console.WriteLine("Bike successfully rented out.");
                Thread.Sleep(1000);
                return;
            }
        }
    }

    public void ReturnBike()
    {
        string? userInput;
        BaseBike? selectedBike = null;
        RentedBike? selectedRental = null;
        bool isMember = false;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Select a bike to return");
            RenderRentedBikes();
            List<BaseBike> rentedBikes = _bikes.Where(b => !b.IsFree).ToList();
            Console.WriteLine();
            Console.WriteLine("0. Back");
            Console.WriteLine();
            Console.Write("-> ");

            userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid input");
                Thread.Sleep(1000);
                continue;
            }

            if (userInput == "0")
            {
                return;
            }

            if (!int.TryParse(userInput, out int index) || index < 1 || index > rentedBikes.Count)
            {
                Console.WriteLine();
                Console.WriteLine("Invalid input");
                Thread.Sleep(1000);
                continue;
            }

            BaseBike bike = rentedBikes[index - 1];
            var rentedBike = _rentedBikes.SingleOrDefault(b => b.BikeId == bike.Id);

            if (rentedBike is null)
            {
                Console.WriteLine();
                Console.WriteLine("Could not find bike.");
                Thread.Sleep(1000);
                return;
            }

            var customer = _customerManager.Customers.SingleOrDefault(c => c.Id == rentedBike.CustomerId);

            if (customer is null)
            {
                Console.WriteLine();
                Console.WriteLine("Could not find customer.");
                Thread.Sleep(1000);
                return;
            }

            selectedBike = bike;
            selectedRental = rentedBike;
            isMember = customer.IsMember;
            break;
        }

        if (selectedBike is null || selectedRental is null)
        {
            return;
        }

        decimal subtotal = selectedBike.CalculatePrice(selectedRental.Length);
        decimal discount = isMember ? subtotal * 0.10m : 0m;
        decimal total = subtotal - discount;
        string bikeType = selectedBike is ElectricBike ? "Electric bike" : "Bike";
        string duration = selectedRental.Length == 1 ? "1 hour" : $"{selectedRental.Length} hours";
        string rentalDescription = $"{bikeType}, {duration} at {selectedBike.PricePerHour.ToString("0.##", CultureInfo.InvariantCulture)} kr";

        if (selectedBike is ElectricBike electricBike)
        {
            electricBike.DrainBattery(selectedRental.Length);
        }

        selectedBike.SetIsFree(true);

        Console.Clear();
        Console.WriteLine("RECEIPT");
        Console.WriteLine(new string('-', 58));
        Console.WriteLine("{0,-42}{1,12} kr", rentalDescription, subtotal.ToString("F2", CultureInfo.InvariantCulture));

        if (isMember)
        {
            Console.WriteLine("{0,-42}{1,12} kr", "Member discount (10%)", $"-{discount.ToString("F2", CultureInfo.InvariantCulture)}");
        }

        Console.WriteLine(new string('-', 58));
        Console.WriteLine("{0,-42}{1,12} kr", "Total due", total.ToString("F2", CultureInfo.InvariantCulture));
        Console.WriteLine();
        Console.Write("Press any key to continue...");
        Console.ReadKey(true);
    }

    public void ChargeBike()
    {
        while (true)
        {
            List<ElectricBike> chargeableBikes = _bikes
                .OfType<ElectricBike>()
                .Where(bike => bike.IsFree && bike.Battery < ElectricBike.MaxBattery)
                .ToList();

            Console.Clear();
            Console.WriteLine("Select an electric bike to charge or cancel charging");
            Console.WriteLine(new string('-', 50));
            Console.WriteLine("{0,-5} {1,-17} {2,-10} {3,-12}", "No.", "Type", "Battery", "Status");
            Console.WriteLine(new string('-', 50));

            for (int i = 0; i < chargeableBikes.Count; i++)
            {
                ElectricBike bike = chargeableBikes[i];
                string status = _chargingBikeIds.TryGetValue(bike.Id, out CancellationTokenSource? chargeStatusTokenSource)
                    ? chargeStatusTokenSource.IsCancellationRequested ? "Cancelling" : "Charging"
                    : "Not charging";

                Console.WriteLine("{0,-5} {1,-17} {2,-10} {3,-12}",
                    i + 1,
                    "Electric bike",
                    $"{bike.Battery}%",
                    status);
            }

            if (chargeableBikes.Count == 0)
            {
                Console.WriteLine("No available electric bikes need charging.");
                Console.WriteLine();
                Console.Write("Press any key to return...");
                Console.ReadKey(true);
                return;
            }

            Console.WriteLine();
            Console.WriteLine("0. Back");
            Console.WriteLine();
            Console.Write("-> ");

            string? userInput = Console.ReadLine();

            if (userInput == "0")
            {
                return;
            }

            if (!int.TryParse(userInput, out int index) || index < 1 || index > chargeableBikes.Count)
            {
                Console.WriteLine();
                Console.WriteLine("Invalid input");
                Thread.Sleep(1000);
                continue;
            }

            ElectricBike selectedBike = chargeableBikes[index - 1];
            if (_chargingBikeIds.TryGetValue(selectedBike.Id, out CancellationTokenSource? activeCancellation))
            {
                activeCancellation.Cancel();
                Console.WriteLine();
                Console.WriteLine("Charging cancellation requested.");
                Thread.Sleep(1000);
                return;
            }

            var cancellationTokenSource = new CancellationTokenSource();
            if (!_chargingBikeIds.TryAdd(selectedBike.Id, cancellationTokenSource))
            {
                cancellationTokenSource.Dispose();
                continue;
            }

            Console.WriteLine();
            Console.WriteLine("Charging started. The bike gains 1% battery every 10 seconds.");
            _ = ChargeBikeInBackground(selectedBike, cancellationTokenSource);
            return;
        }
    }

    private async Task ChargeBikeInBackground(ElectricBike bike, CancellationTokenSource cancellationTokenSource)
    {
        try
        {
            while (bike.Battery < ElectricBike.MaxBattery)
            {
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationTokenSource.Token);

                if (bike.ChargeBatteryByOne())
                {
                    break;
                }
            }
        }
        catch (OperationCanceledException) when (cancellationTokenSource.IsCancellationRequested)
        {
        }
        finally
        {
            _chargingBikeIds.TryRemove(bike.Id, out _);
        }
    }

    private void RenderCustomers()
    {
        IReadOnlyList<Customer> customers = _customerManager.Customers;

        Console.WriteLine(new string('-', 48));
        Console.WriteLine("{0,-3} {1,-24} {2,-6} {3,-10}",
            "No.", "Name", "Age", "Member");
        Console.WriteLine(new string('-', 48));

        for (int i = 0; i < customers.Count; i++)
        {
            Customer customer = customers[i];
            Console.WriteLine("{0,-3} {1,-24} {2,-6} {3,-10}",
                i + 1,
                customer.Name,
                customer.Age,
                customer.IsMember ? "Yes" : "No");
        }
    }
}
