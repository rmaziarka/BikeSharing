// The Azure Cosmos DB endpoint for running this sample.

using System.Configuration;
using BikeSharing.Console.Case1;
using Microsoft.Azure.Cosmos;

string endpointUri = ConfigurationManager.AppSettings.Get("EndpointUri")!;
string primaryKey = ConfigurationManager.AppSettings.Get("PrimaryKey")!;

// https://devblogs.microsoft.com/cosmosdb/introducing-bulk-support-in-the-net-sdk/
var cosmosClient = new CosmosClient(endpointUri, primaryKey, new CosmosClientOptions(){ AllowBulkExecution = true });



// [GENERATOR]
//await BikesGenerator.GenerateBikes(cosmosClient);

await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,27), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,26), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,25), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,24), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,23), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,22), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,21), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,20), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,19), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,18), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,17), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,16), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,15), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,14), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,13), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,12), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,11), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,10), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,9), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,8), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,7), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,6), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,5), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,4), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,3), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,2), true);
await RentalGenerator.GenerateReservationsAndRentals(cosmosClient, new DateOnly(2023,3,1), true);



// [RESERVATION SCENARIOS]
// [Get bikes around]
// await ReservationScenarios.GetBikesAround(cosmosClient);

// [Reserve free standing bike]
// Guid bikeAvailabilityId = Guid.Parse("051c860a-38ee-4edb-bbe3-ba3c48e23685");
// string clientId = "user-0-775";
// await ReservationScenarios.ReserveFreeStandingBike(cosmosClient, bikeAvailabilityId, clientId);

// [Get bikes from station]
// TODO

// [Reserve first bike from station]
// TODO




// [RENTAL SCENARIOS]
// [Create rental without reservation]
// TODO

// [Get active reservations]
// TODO

// [Create rental based on reservation] 
// var reservationId = Guid.Parse("8def08d1-bbb5-49be-bf0c-4e0061aca82c"); //get previously created reservation
// string customerId = "user-0-775";
// await RentalScenarios.MakeRentalBasedOnReservation(cosmosClient,customerId, reservationId);


// [RETURN SCENARIOS]
// [Get active rentals for user]
// TODO

// [Finish rental]
// TODO

// Finish rental in station
// var bikeAvailabilityId = Guid.Parse("051c860a-38ee-4edb-bbe3-ba3c48e23685");
// var stationId = Guid.Parse("b68b41db-8f78-4962-955c-0d8e6e5121f8");
// await ReturnScenarios.ReturnBikeInStation(cosmosClient, bikeAvailabilityId, stationId); 
