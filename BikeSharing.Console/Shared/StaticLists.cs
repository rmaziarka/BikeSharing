using BikeSharing.Console.SharedModels;
using Newtonsoft.Json;

namespace BikeSharing.Console.Shared;

public class StaticLists
{
    public static List<City> Cities { get; }
    public static Dictionary<Guid, City> CitiesDict { get; } = new Dictionary<Guid, City>();

    public static List<Station> Stations { get; }
    public static Dictionary<Guid, Station> StationsDict { get; } = new Dictionary<Guid, Station>();
    
    
    public static List<Bike> Bikes { get; }
    
    public static Dictionary<Guid, Bike> BikesDict { get; } = new Dictionary<Guid, Bike>();
    

    static StaticLists()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var citiesPath = Path.Combine(currentDirectory, "cities.json");
        var stationsPath = Path.Combine(currentDirectory, "stations.json");
        var bikesPath = Path.Combine(currentDirectory, "bikes.json");
        
        
        Cities = JsonConvert.DeserializeObject<List<City>>(File.ReadAllText(citiesPath));
        Stations = JsonConvert.DeserializeObject<List<Station>>(File.ReadAllText(stationsPath));
        Bikes =  JsonConvert.DeserializeObject<List<Bike>>(File.ReadAllText(bikesPath));

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

        foreach (var bike in Bikes)
        {
            BikesDict[bike.Id] = bike;
            
            var city = CitiesDict[bike.CityId];
            city.Bikes.Add(bike);
        }
    }
}