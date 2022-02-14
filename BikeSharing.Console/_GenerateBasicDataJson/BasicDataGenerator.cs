using BikeSharing.Console.Case1;
using BikeSharing.Console.Shared;
using BikeSharing.Console.SharedModels;
using Microsoft.Azure.Cosmos.Spatial;
using Newtonsoft.Json;

namespace BikeSharing.Console._GenerateBasicDataJson;

public class BasicDataGenerator
{
    private static List<City> Cities { get; set; }
    
    private static List<Station> Stations { get; set; }
    
    private static List<Client> Clients { get; set; }

    private static List<Bike> Bikes { get; set; }
    

    public static void GenerateCitiesAndStations()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var file = "cities_github.json";
        var path = Path.Combine(currentDirectory, file);

        var cities = JsonConvert.DeserializeObject<List<JsonCity>>(File.ReadAllText(path));

        Cities = cities.Take(100).Select(c => new City()
            {
                Id = Guid.NewGuid(),
                Name = c.city,
                Location = new Point(double.Parse(c.lng), double.Parse(c.lat)),
                Population = int.Parse(c.population)
            }
        ).ToList();

        // to create linear distribution between 400X and X between 100 elements
        var loweringDiscriminator = 0.0097;
        
        var numberOfStations = 400;
        
        // 0.1 long&lat = 11km = +/- warsaw radius
        var longLatMaxRadius = 0.1;
        var random = new Random();

        Stations = new List<Station>();
        
        for (int i = 0; i < 100; i++)
        {
            var city = Cities[i];

            var cityStationsNumber = (int) numberOfStations * (1 - i * loweringDiscriminator); 
            
            var longLatRadius = (double) longLatMaxRadius * (1 - i * loweringDiscriminator); 

            for (int j = 0; j < cityStationsNumber; j++)
            {
                var stationName = "Station " + j;

                var station = new Station() {Id = Guid.NewGuid(), Name = stationName, CityId = city.Id};
                
                // generate station location based on the normal distribution
                var stationLatitude = city.Location.Position.Latitude + random.NextGaussian(0, longLatRadius/3);
                var stationLongitude = city.Location.Position.Longitude + random.NextGaussian(0, longLatRadius/3);

                station.Location = new Point(stationLongitude, stationLatitude);
                
                Stations.Add(station);
            }
        }

        var citiesString = JsonConvert.SerializeObject(Cities);
        var stationsString = JsonConvert.SerializeObject(Stations);
        var citiesPath = Path.Combine(currentDirectory, "cities.json");
        var stationsPath = Path.Combine(currentDirectory, "stations.json");

        File.WriteAllText(citiesPath, citiesString);
        
        File.WriteAllText(stationsPath, stationsString);
    }

    public static void GenerateBikes()
    {
        Bikes = new List<Bike>();
        var globalBikeIndex = 0;
        for (int cityIndex = 0; cityIndex < StaticLists.Cities.Count; cityIndex++)
        {
            var numberOfBikesForCity = Constants.GetNumberOfBikesForCity(cityIndex);

            for (int localBikeIndex = 0; localBikeIndex < numberOfBikesForCity; localBikeIndex++)
            {
                var city = StaticLists.Cities[cityIndex];
                
                var bike = new Bike()
                {
                    Id = Guid.NewGuid(),
                    Name = $"Bike-{globalBikeIndex}-{localBikeIndex}",
                    CityId = city.Id
                };
                
                Bikes.Add(bike);

                globalBikeIndex++;
            }
        }
        
        var currentDirectory = Directory.GetCurrentDirectory();
        var bikesString = JsonConvert.SerializeObject(Bikes);
        var bikesPath = Path.Combine(currentDirectory, "bikes.json");

        File.WriteAllText(bikesPath, bikesString);
    }

    public static void GenerateClients()
    {
        Clients = new List<Client>();
        var globalClientIndex = 0;
        for (int cityIndex = 0; cityIndex < StaticLists.Cities.Count; cityIndex++)
        {
            var numberOfClients = Constants.GetNumberOfClientsForCity(cityIndex);

            for (int localClientIndex = 0; localClientIndex < numberOfClients; localClientIndex++)
            {
                var city = StaticLists.Cities[cityIndex];
                
                var client = new Client()
                {
                    Id = Guid.NewGuid(),
                    FirstName = $"Local{localClientIndex}",
                    LastName = $"Global{globalClientIndex}",
                    CityId = city.Id
                };
                
                Clients.Add(client);

                globalClientIndex++;
            }
        }
        
        var currentDirectory = Directory.GetCurrentDirectory();
        var clientsString = JsonConvert.SerializeObject(Clients);
        var clientsPath = Path.Combine(currentDirectory, "clients.json");

        File.WriteAllText(clientsPath, clientsString);
    }
}

public class JsonCity
{
    public string city { get; set; }
    public string lat { get; set; }
    
    public string lng { get; set; }
    public string population { get; set; }
}
