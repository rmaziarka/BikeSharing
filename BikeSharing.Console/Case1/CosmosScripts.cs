using Microsoft.Azure.Cosmos;

namespace BikeSharing.Console.Case1;
using static System.Linq.Enumerable;
public static class CosmosScripts
{
    public static string Database = "case1";

    public static string BikeContainerName = "bikes";
    public static string RentalContainerName = "rentals";
    public static async Task GenerateBikes(CosmosClient cosmosClient)
    {
        var bikesContainer = cosmosClient.GetContainer(Database, BikeContainerName);

        var random = new Random();
        var bikeRUCharge = 0.0;
        for (int cityIndex = 0; cityIndex < StaticLists.Cities.Count; cityIndex++)
        {
            var city = StaticLists.Cities[cityIndex];

            var numberOfBikes = Constants.GetNumberOfBikesForCity(cityIndex);

            var bikes = new List<Bike>();
            foreach (var bikeIndex in Range(0,numberOfBikes))
            {
                var bikeType = bikeIndex % 30 == 0
                    ? BikeType.Electrical
                    : BikeType.Traditional;
                
                var bike = new Bike()
                {
                    Id = Guid.NewGuid(),
                    CityId = city.Id,
                    IsTaken = false,
                    BikeType = bikeType
                };

                // free-standing bike - every 1 of 20
                if (bikeIndex % 20 == 0)
                {
                    bike.Location = random.GeneratePoint(
                        cityIndex, 
                        city.Location.Position.Latitude,
                        city.Location.Position.Longitude
                    );
                }
                // station bike
                else
                {
                    // linear distribution of bikes between stations
                    var stationIndex = bikeIndex % city.Stations.Count;
                    var station = city.Stations[stationIndex];
                    bike.Location = station.Location;
                    bike.StationId = station.Id;
                }
                
                bikes.Add(bike);
            }

            var cityRUCharge = 0.0;
            foreach (var bikesBatch in bikes.BatchBy(100))
            {
                TransactionalBatch batch = bikesContainer.CreateTransactionalBatch(new PartitionKey(city.Id.ToString()));

                foreach (var bike in bikesBatch)
                {
                    batch.CreateItem(bike);
                }
                TransactionalBatchResponse batchResponse = await batch.ExecuteAsync();
                cityRUCharge += batchResponse.RequestCharge;
                
                using (batchResponse)
                {
                    if (!batchResponse.IsSuccessStatusCode)
                    {
                        System.Console.WriteLine("Nieudało się");
                    }
                }
            }

            bikeRUCharge += cityRUCharge;
            System.Console.WriteLine($"RU cost for adding bikes to {city.Name} :{cityRUCharge:F2}");
            System.Console.WriteLine($"Cost per bike {(cityRUCharge/ numberOfBikes):F2}");
        }
        System.Console.WriteLine($"RU cost for adding all bikes to {StaticLists.Cities.Count} cities :{bikeRUCharge:F2}");
    }

    public static async Task GenerateBike(CosmosClient cosmosClient)
    {
        var bikesContainer = cosmosClient.GetContainer(Database, BikeContainerName);
        var warsaw = StaticLists.Cities.First();
        var station = warsaw.Stations.First();

        var bike = new Bike()
        {
            BikeType = BikeType.Traditional,
            CityId = warsaw.Id,
            StationId = station.Id,
            IsTaken = false,
            Id = Guid.Parse("9693668c-7132-47ec-b752-c3e43d86b1d1")
        };

        var result = await bikesContainer.CreateItemAsync(bike);
        System.Console.WriteLine($"RU charge for a single bike {result.RequestCharge:F2}");
    } 
}