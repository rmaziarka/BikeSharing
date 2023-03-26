using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BikeSharing.Exercise.Shared.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace BikeSharing.Exercise.Functions;


public static class ClientsFunction
{
    private static readonly Container _rentalsContainer;

    static ClientsFunction()
    {
        string connectionString = Environment.GetEnvironmentVariable("CosmosDBConnectionSetting", EnvironmentVariableTarget.Process);
        var cosmosClient = new CosmosClient(connectionString, new CosmosClientOptions(){ AllowBulkExecution = true });
        _rentalsContainer = cosmosClient.GetContainer("database", "clients");
    }
    
    [FunctionName("ClientsFunction")]
    public static async Task Run([CosmosDBTrigger(
        databaseName: "database",
        containerName: "rentals",
        Connection = "CosmosDBConnectionSetting",
        LeaseContainerName = "leases",
        CreateLeaseContainerIfNotExists = true)]JArray documents, ILogger log)
    {
        if(documents == null || documents.Count == 0) return;
    
        foreach (JObject document in documents)
        {
            var type = (string) document.Property("Type");
            if (type != "Rental") return;
            
            var rental = document.ToObject<Rental>();
            
            // retrieve the client from the database
            
            // update the client with the new rental
            
            // save the client to the database
        }
            
    }
}