using BikeSharing.Console.SharedModels;
using Newtonsoft.Json;

namespace BikeSharing.Console.Shared;

public class StaticLists
{
    public static List<City> Cities { get; }
    public static Dictionary<Guid, City> CitiesDict { get; } = new Dictionary<Guid, City>();

    public static List<Station> Stations { get; }
    public static Dictionary<Guid, Station> StationsDict { get; } = new Dictionary<Guid, Station>();

    static StaticLists()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var citiesPath = Path.Combine(currentDirectory, "cities.json");
        var stationsPath = Path.Combine(currentDirectory, "stations.json");
        
        
        Cities = JsonConvert.DeserializeObject<List<City>>(File.ReadAllText(citiesPath));
        Stations = JsonConvert.DeserializeObject<List<Station>>(File.ReadAllText(stationsPath));

        foreach (var city in Cities)
        {
            CitiesDict[city.Id] = city;
        }

        foreach (var station in Stations)
        {
            StationsDict[station.Id] = station;
            
            var city = CitiesDict[station.CityId];
            city.Stations.Add(station);
        }
    }
}