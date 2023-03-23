using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Spatial;

namespace BikeSharing.Exercise.Console.Scenarios;

public static class ReservationScenarios
{
    public static string Database = "database";

    public static string AvailabilityContainerName = "availability";
    public static string RentalContainerName = "rentals";

    
    public static async Task GetBikesAround(CosmosClient cosmosClient, Guid cityId, Point location)
    {
        var requestCharge = 0.0;
        var availabilityContainer = cosmosClient.GetContainer(Database, AvailabilityContainerName);
        
        // create query
        // .Where(a => a.Location.Distance(ClientLocation) < 1000)
        
        // run query to get bikes availability

        // Console.WriteLine($"[Q] GetBikesAround - {bikes.Count} Items, {requestCharge:F2} RU");

    }

    public static async Task ReserveFreeStandingBike(CosmosClient cosmosClient, Guid cityId, Guid clientId, Guid bikeAvailabilityId)
    {
        var requestCharge = 0.0;
        var availabilityContainer = cosmosClient.GetContainer(Database, AvailabilityContainerName);
        var rentalsContainer = cosmosClient.GetContainer(Database, RentalContainerName);
        
        // create reservation object
        
        // create reservation in db
        
        // Console.WriteLine($"[C] ReserveFreeStandingBike - {requestCharge:F2} RU");
        // Console.WriteLine($"Created reservation {reservation.Id} Id");
    }

    
    // {
    //      // retrieve bike availability
    //
    //      // create reservation object
    //     
    //      // set owner for bike availability
    //     
    //      // update bike availability in db
    // ItemRequestOptions requestOptions = new ItemRequestOptions { IfMatchEtag = firstResponse.ETag };
    // var updateResponse = await container.UpsertItemAsync(object, requestOptions: requestOptions);
    // if (updateResponse.StatusCode == HttpStatusCode.PreconditionFailed)
    // {
    //     Console.WriteLine("");
    //     return;
    // }
        
    //      // create reservation in db
    // }
}