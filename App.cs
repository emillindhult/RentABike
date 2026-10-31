using RentABike.Managers;

namespace RentABike;

internal sealed class App
{
    private readonly CustomerManager _customerManager;
    private readonly BikeManager _bikeManager;

    public App()
    {
        _customerManager = new CustomerManager();
        _bikeManager = new BikeManager();
        Console.CursorVisible = false;
    }

    public void Run()
    {
        MainMenu();
    }

    private void MainMenu()
    {
        string? userInput;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("==== Rent a Bike ====");
            Console.WriteLine("---------------------");
            Console.WriteLine("1. List vehicles");
            Console.WriteLine("2. Register customer");
            Console.WriteLine("3. Rent out");
            Console.WriteLine("4. Return");
            Console.WriteLine("5. Charge");
            Console.WriteLine("0. Exit");
            Console.WriteLine();
            Console.Write("-> ");

            userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid input...");
                Thread.Sleep(1000);
            }

            switch (userInput)
            {
                case "1":
                    ListVehicles();
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    break;
                case "5":
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
            }
        }
    }

    private void ListVehicles()
    {
        _bikeManager.ListBikes();
    }
}
