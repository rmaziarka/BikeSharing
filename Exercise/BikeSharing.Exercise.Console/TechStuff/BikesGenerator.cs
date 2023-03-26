﻿using BikeSharing.Exercise.Shared.Models;
using Microsoft.Azure.Cosmos;

namespace BikeSharing.Exercise.Console.TechStuff;

public static class BikesGenerator
{
    public static string Database = "database";

    public static string AvailabilityContainerName = "availability";
    public static async Task GenerateBikes(CosmosClient cosmosClient, bool warsawOnly = true)
    {
        var availabilityContainer = cosmosClient.GetContainer(Database, AvailabilityContainerName);

        var random = new Random();
        var bikeRUCharge = 0.0;
        for (int cityIndex = 0; cityIndex < StaticLists.Cities.Count; cityIndex++)
        {
            var city = StaticLists.Cities[cityIndex];

            var numberOfBikes = city.Bikes.Count;

            var bikeAvailabilities = new List<BikeAvailability>();
            for (int bikeIndex = 0; bikeIndex < city.Bikes.Count; bikeIndex++)
            {
                var bike = city.Bikes[bikeIndex];
                
                var bikeType = bikeIndex % 30 == 0
                    ? BikeType.Electrical
                    : BikeType.Regular;
                
                var bikeAvailability = new BikeAvailability()
                {
                    Id = bike.Id,
                    CityId = city.Id,
                    BikeType = bikeType
                };

                // free-standing bike - every 1 of 10
                if (bikeIndex % 10 == 0)
                {
                    // normal distribution of bikes in city
                    bikeAvailability.Location = random.GetNormalRandomPointInCity(
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
                    bikeAvailability.Location = station.Location;
                    bikeAvailability.StationId = station.Id;
                }
                
                bikeAvailabilities.Add(bikeAvailability);
            }
            
            var cityRUCharge = 0.0;
            
            // could use TransactionBatch but it is slower
            List<Task> concurrentTasks = new List<Task>();
            foreach(var bikeAvailability in bikeAvailabilities)
            {
                concurrentTasks.Add(availabilityContainer.CreateItemAsync(bikeAvailability,
                    new PartitionKey(bikeAvailability.CityId.ToString())));
            }

            await Task.WhenAll(concurrentTasks);
            
            foreach (var task in concurrentTasks)
            {
                var response = ((Task<ItemResponse<BikeAvailability>>)task).Result;
                cityRUCharge += response.RequestCharge;
            }

            bikeRUCharge += cityRUCharge;
            System.Console.WriteLine($"RU cost for adding bikes to {city.Name} :{cityRUCharge:F2}");
            System.Console.WriteLine($"Cost per bike {(cityRUCharge/ numberOfBikes):F2}");

            if(warsawOnly)
                return;
        }
        System.Console.WriteLine($"RU cost for adding all bikes to {StaticLists.Cities.Count} cities :{bikeRUCharge:F2}");
    }
}