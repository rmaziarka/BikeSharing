using System.Net;
using BikeSharing.Console.Case1.Models;
using BikeSharing.Console.Shared;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.Cosmos.Spatial;

public static class RentalScenarios
{
    public static string Database = "case1";

    public static string AvailabilityContainerName = "availability";
    public static string RentalContainerName = "rentals";

    public static Guid CityId = StaticLists.Cities.First().Id;
    
    
    
    
    public static async Task MakeRentalBasedOnReservation(CosmosClient cosmosClient, string clientId, Guid reservationId)
    {
        var requestCharge = 0.0;
        
        var availabilityContainer = cosmosClient.GetContainer(Database, AvailabilityContainerName);
        
        var rentalsContainer = cosmosClient.GetContainer(Database, RentalContainerName);

        
        
        var reservationResponse =
            await rentalsContainer.ReadItemAsync<Reservation>(
                reservationId.ToString(),
                new PartitionKey(clientId)
            );
        
        requestCharge += reservationResponse.RequestCharge;
        
        var reservation = reservationResponse.Resource;
        
        
        
        var bikeAvailabilityResponse =
            await availabilityContainer.ReadItemAsync<BikeAvailability>(
                reservation.BikeId.ToString(),
                new PartitionKey(CityId.ToString())
            );
        
        requestCharge += bikeAvailabilityResponse.RequestCharge;
        
        var bikeAvailability = bikeAvailabilityResponse.Resource;

        
        
        var rental = new Rental()
        {
            Id = Guid.NewGuid(),
            BikeId = reservation.BikeId,
            CityId = CityId,
            ClientId = clientId,
            StartDate = DateTime.Now,
            ExpirationDate = DateTime.Now.AddMinutes(15),
            BasedOn = new BasedOn(){Id = reservationId, Type = BasedOnType.Reservation}
        };

        reservation.Completed = new Completed() {Date = DateTime.Now, RentalId = rental.Id};
        bikeAvailability.Owner = new Owner() {Id = rental.Id, Type = OwnerType.Rental, ClientId = clientId};

        
        // https://docs.microsoft.com/en-us/azure/cosmos-db/sql/transactional-batch
        TransactionalBatch batch = rentalsContainer.CreateTransactionalBatch(new PartitionKey(clientId))
            .CreateItem(rental)
            .ReplaceItem(reservationId.ToString(), reservation);
        
        TransactionalBatchResponse batchResponse = await batch.ExecuteAsync();
        
        requestCharge += batchResponse.RequestCharge;
        
        using (batchResponse)
        {
            if (!batchResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(batchResponse.ErrorMessage);
            }
        }

        

        var updateBikeAvailabilityResponse =
            await availabilityContainer.ReplaceItemAsync(
                bikeAvailability, bikeAvailability.Id.ToString(), 
                new PartitionKey(CityId.ToString()));
        
        requestCharge += updateBikeAvailabilityResponse.RequestCharge;
        
        
        
        Console.WriteLine($"[C] MakeRentalBasedOnReservation - {requestCharge:F2} RU");
        Console.WriteLine($"Created rental {rental.Id} Id");
    }
}