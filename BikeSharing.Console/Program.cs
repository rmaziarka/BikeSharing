// The Azure Cosmos DB endpoint for running this sample.

using System.Configuration;
using BikeSharing.Console._GenerateBasicDataJson;
using BikeSharing.Console.Case1;
using BikeSharing.Console.Shared;
using Microsoft.Azure.Cosmos;

string endpointUri = ConfigurationManager.AppSettings.Get("EndpointUri")!;
string primaryKey = ConfigurationManager.AppSettings.Get("PrimaryKey")!;

//var cosmosClient = new CosmosClient(endpointUri, primaryKey);

// await BikesGenerator.GenerateBikes(cosmosClient);

BasicDataGenerator.GenerateBikes();

//var bike = StaticLists.Bikes.First();

//Console.WriteLine($"{bike.Name}, {bike.CityId}");