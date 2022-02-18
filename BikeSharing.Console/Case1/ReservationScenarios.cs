using System.Net;
using BikeSharing.Console.Case1.Models;
using BikeSharing.Console.Shared;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.Cosmos.Spatial;

public static class ReservationScenarios
{
    public static string Database = "case1";

    public static string AvailabilityContainerName = "availability";
    public static string RentalContainerName = "rentals";

    public static Point ClientLocation = new Point(20.986339, 52.245834);
    public static Guid CityId = StaticLists.Cities.First().Id;
    
    public static async Task GetBikesAround(CosmosClient cosmosClient)
    {
        var availabilityContainer = cosmosClient.GetContainer(Database, AvailabilityContainerName);
        
        var query = availabilityContainer
            .GetItemLinqQueryable<BikeAvailability>()
            .Where(a => a.CityId == CityId)
            .Where(a => a.Location.Distance(ClientLocation) < 1000)
            .Where(a => !a.IsTaken);
        
        FeedIterator<BikeAvailability> queryResultSetIterator = query.ToFeedIterator();

        List<BikeAvailability> bikes = new List<BikeAvailability>();

        var requestCharge = 0.0;
        while (queryResultSetIterator.HasMoreResults)
        {
            FeedResponse<BikeAvailability> currentResultSet = await queryResultSetIterator.ReadNextAsync();
            foreach (BikeAvailability family in currentResultSet)
            {
                bikes.Add(family);
            }

            requestCharge += currentResultSet.RequestCharge;
        }
        
        Console.WriteLine($"[Q] GetBikesAround - {bikes.Count} Items, {requestCharge:F2} RU");

        // return bikes;
    }

    public static async Task ReserveFreeStandingBike(CosmosClient cosmosClient, Guid bikeAvailabilityId,
        string clientId)
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
        var freeStandingBike = bikeAvailabilityResponse.Resource;

        var reservation = new Reservation()
        {
            Id = Guid.NewGuid(),
            BikeId = bikeAvailabilityId,
            CityId = CityId,
            ClientId = clientId,
            StartDate = DateTime.Now,
            ExpirationDate = DateTime.Now.AddMinutes(15)
        };
        
        freeStandingBike.Owner = new Owner() {Id = reservation.Id, Type = OwnerType.Reservation, ClientId = clientId};
        freeStandingBike.Location = null;
        freeStandingBike.StationId = null;
        
        ItemRequestOptions requestOptions = new ItemRequestOptions { IfMatchEtag = bikeAvailabilityResponse.ETag };
        var updateResponse = await availabilityContainer.UpsertItemAsync(freeStandingBike, requestOptions: requestOptions);
        if (updateResponse.StatusCode == HttpStatusCode.PreconditionFailed)
        {
            Console.WriteLine("Somebody else already reserved/rented this bike");
            return;
        }
        requestCharge += updateResponse.RequestCharge;
        
        var reservationResponse = await rentalsContainer.CreateItemAsync(reservation);
        requestCharge += reservationResponse.RequestCharge;
        
        Console.WriteLine($"[C] ReserveFreeStandingBike - {requestCharge:F2} RU");
        Console.WriteLine($"Created reservation {reservation.Id} Id");
    }
    
    
    public static async Task GetBikesFromStation(CosmosClient cosmosClient, Guid stationId)
    {
        var availabilityContainer = cosmosClient.GetContainer(Database, AvailabilityContainerName);

        var query = availabilityContainer
            .GetItemLinqQueryable<BikeAvailability>()
            .Where(a => a.CityId == CityId && a.StationId == stationId)
            .Where(a => !a.IsTaken);
        
        FeedIterator<BikeAvailability> queryResultSetIterator = query.ToFeedIterator();

        List<BikeAvailability> bikes = new List<BikeAvailability>();

        var requestCharge = 0.0;
        while (queryResultSetIterator.HasMoreResults)
        {
            FeedResponse<BikeAvailability> currentResultSet = await queryResultSetIterator.ReadNextAsync();
            foreach (BikeAvailability family in currentResultSet)
            {
                bikes.Add(family);
            }

            requestCharge += currentResultSet.RequestCharge;
        }
        
        Console.WriteLine($"[Q] GetBikesFromStation - {bikes.Count} Items, {requestCharge:F2} RU");

        // return bikes.GroupBy(el => el.BikeType);
    }
}