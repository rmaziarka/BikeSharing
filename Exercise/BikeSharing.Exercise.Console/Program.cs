using System.Configuration;
using BikeSharing.Exercise.Console.Scenarios;
using BikeSharing.Exercise.Console.TechStuff;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Spatial;

// [STATIC]
Guid cityId = Guid.Parse("98344ca0-6a9f-47e9-a5ae-99b1e1319db2"); // warsaw
Guid clientId = Guid.Parse("b5d89a60-d81b-43eb-9c2a-91217ea148f2");
Point clientLocation = new Point(20.986339, 52.245834); // center of warsaw
Guid bikeAvailabilityId = Guid.Parse("c4abcc38-ac80-48c2-904f-02f24b7d9a82"); // bike from warsaw

string connectionString = ConfigurationManager.AppSettings.Get("CosmosDBConnectionSetting")!;
var cosmosClient = new CosmosClient(connectionString, new CosmosClientOptions(){ AllowBulkExecution = true });

// [GENERATOR]
await BikesGenerator.GenerateBikes(cosmosClient);
await ClientsGenerator.GenerateClients(cosmosClient);



// [CLIENT SCENARIOS]

// [Get clients from city]
await ClientScenarios.GetClientsInCity(cosmosClient, cityId);
 
// [Add client]
var newClientFirstName = "Adam";
var newClientLastName = "Kowalski";
var newClientId = Guid.NewGuid();
await ClientScenarios.AddClient(cosmosClient, cityId, clientId,newClientFirstName, newClientLastName);

 // [Add rental to client]
var rentalId = Guid.NewGuid();
var rentalStartDate = DateTime.Now;
await ClientScenarios.AddRentalToClient(cosmosClient, cityId, clientId,rentalId, rentalStartDate);



// [RESERVATION SCENARIOS]

// [Get bikes around]
// await ReservationScenarios.GetBikesAround(cosmosClient, cityId, clientLocation);

// [Reserve free standing bike]
// await ReservationScenarios.ReserveFreeStandingBike(cosmosClient, cityId, clientId, bikeAvailabilityId);



// [RENTAL SCENARIOS]
var reservationId = Guid.Parse(""); //get previously created reservation
await RentalScenarios.MakeRentalBasedOnReservation(cosmosClient, cityId, reservationId);


