using System.Net;
using BikeSharing.Console.Case1.Models;
using BikeSharing.Console.Shared;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.Cosmos.Spatial;

public static class ReturnScenarios
{
    public static string Database = "case1";

    public static string AvailabilityContainerName = "availability";
    public static string RentalContainerName = "rentals";

    public static Point ClientLocation = new Point(20.986339, 52.245834);
    public static Guid CityId = StaticLists.Cities.First().Id;

    public static async Task ReturnBikeInStation(CosmosClient cosmosClient, Guid bikeAvailabilityId, Guid stationId)
    {
        var requestCharge = 0.0;
        var availabilityContainer = cosmosClient.GetContainer(Database, AvailabilityContainerName);
        var rentalsContainer = cosmosClient.GetContainer(Database, RentalContainerName);

        
        
        var bikeAvailabilityResponse =
            await availabilityContainer.ReadItemAsync<BikeAvailability>(
                bikeAvailabilityId.ToString(),
                new PartitionKey(CityId.ToString())
            );
        requestCharge += bikeAvailabilityResponse.RequestCharge;
        var bikeAvailability = bikeAvailabilityResponse.Resource;
        var clientId = bikeAvailability.Owner.ClientId;

        
        
        var query = rentalsContainer
            .GetItemLinqQueryable<Rental>(requestOptions: new QueryRequestOptions() {MaxItemCount = 1})
            .Where(r => r.ClientId == clientId && r.Id == bikeAvailability.Owner.Id)
            .ToFeedIterator();
        var rentalResponse = await query.ReadNextAsync();
        requestCharge += rentalResponse.RequestCharge;
        var rental = rentalResponse.Resource.First();

        
        
        bikeAvailability.Owner = null;
        bikeAvailability.StationId = stationId;
        bikeAvailability.Location = StaticLists.StationsDict[stationId].Location;
        rental.CompletedDate = DateTime.Now;
     
        var updateBikeAvailabilityResponse = await availabilityContainer.ReplaceItemAsync(bikeAvailability, bikeAvailability.Id.ToString(), new PartitionKey(CityId.ToString()));
        requestCharge += updateBikeAvailabilityResponse.RequestCharge;
        var updateRentalResponse = await rentalsContainer.ReplaceItemAsync(rental, rental.Id.ToString(), new PartitionKey(clientId));
        requestCharge += updateRentalResponse.RequestCharge;
        
        
        
        Console.WriteLine($"[C] ReturnBikeInStation - {requestCharge:F2} RU");
    }
}