using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RentABike.Models.Bike;

namespace RentABike;

internal static class FileHandler
{
    public static void AddRangeItems(List<BaseBike> bikes)
    {
        ArgumentNullException.ThrowIfNull(bikes);

        if (bikes.Count == 0)
        {
            return;
        }

        string filePath = GetFilePath();
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        JArray bikeArray = File.Exists(filePath)
            ? JArray.Parse(File.ReadAllText(filePath))
            : new JArray();

        foreach (BaseBike bike in bikes)
        {
            JObject bikeObject = JObject.FromObject(bike);
            bikeObject.Add("BikeType", bike.GetType().Name);
            bikeArray.Add(bikeObject);
        }

        File.WriteAllText(filePath, bikeArray.ToString(Formatting.Indented));
    }

    public static List<BaseBike> ReadItems()
    {
        string filePath = GetFilePath();
        if (!File.Exists(filePath))
        {
            return [];
        }

        JArray bikeArray = JArray.Parse(File.ReadAllText(filePath));
        List<BaseBike> bikes = [];

        foreach (JToken token in bikeArray)
        {
            if (token is not JObject bikeObject)
            {
                throw new JsonSerializationException("Each bike entry must be a JSON object.");
            }

            string? bikeType = bikeObject.Value<string>("BikeType");
            BaseBike bike = bikeType switch
            {
                nameof(Bike) => bikeObject.ToObject<Bike>()
                    ?? throw new JsonSerializationException("Could not read bike entry."),
                nameof(ElectricBike) => bikeObject.ToObject<ElectricBike>()
                    ?? throw new JsonSerializationException("Could not read electric bike entry."),
                _ => throw new JsonSerializationException($"Unknown bike type '{bikeType}'.")
            };

            bikes.Add(bike);
        }

        return bikes;
    }

    private static string GetFilePath()
    {
        string dataDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "RentABike");

        return Path.Combine(dataDirectory, "bikes.json");
    }
}
