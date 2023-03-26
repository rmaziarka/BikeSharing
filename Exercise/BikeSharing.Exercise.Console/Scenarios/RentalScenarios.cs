using Microsoft.Azure.Cosmos;

namespace BikeSharing.Exercise.Console.Scenarios;

public static class RentalScenarios
{
    public static string Database = "database";

    public static string AvailabilityContainerName = "availability";
    public static string RentalContainerName = "rentals";


    public static async Task MakeRentalBasedOnReservation(CosmosClient cosmosClient, Guid clientId, Guid reservationId)
    {
        var requestCharge = 0.0;

        var availabilityContainer = cosmosClient.GetContainer(Database, AvailabilityContainerName);
        var rentalsContainer = cosmosClient.GetContainer(Database, RentalContainerName);

        // retrieve reservation from db
        
        // create rental object
        // make reservation completed
        // set rental as owner for bike availability
        
        
        // save rental and reservation in db
        // https://docs.microsoft.com/en-us/azure/cosmos-db/sql/transactional-batch
        //TransactionalBatch batch = rentalsContainer.CreateTransactionalBatch(new PartitionKey(partitionKey))
        //    .CreateItem(object)
        //    .ReplaceItem(anotherObj.ToString(), anotherObj);
        //TransactionalBatchResponse batchResponse = await batch.ExecuteAsync();
        
        
        // using (batchResponse)
        // {
        //     if (!batchResponse.IsSuccessStatusCode)
        //     {
        //         Console.WriteLine(batchResponse.ErrorMessage);
        //     }
        // }

        
        //Console.WriteLine($"[C] MakeRentalBasedOnReservation - {requestCharge:F2} RU");
        //Console.WriteLine($"Created rental {rental.Id} Id");
    }
}