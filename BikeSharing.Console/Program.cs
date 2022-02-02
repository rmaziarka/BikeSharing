

// The Azure Cosmos DB endpoint for running this sample.

using System.Configuration;
using BikeSharing.Console.Case1;
using Microsoft.Azure.Cosmos;

string endpointUri = ConfigurationManager.AppSettings.Get("EndpointUri")!;
string primaryKey = ConfigurationManager.AppSettings.Get("PrimaryKey")!;

var cosmosClient = new CosmosClient(endpointUri, primaryKey);

await CosmosScripts.GenerateBike(cosmosClient);