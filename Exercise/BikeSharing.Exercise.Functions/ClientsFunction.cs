using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        _rentalsContainer = cosmosClient.GetContainer("database", "rentals");

    }
    
    [FunctionName("ClientsFunction")]
    public static async Task Run([CosmosDBTrigger(
        databaseName: "case1",
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
        }
            
    }
}

public class Rental
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    public string Type => nameof(Rental);
    
    public DateTime StartDate { get; set; }
    
    public DateTime ExpirationDate { get; set; }
    
    public Guid BikeId { get; set; }
    
    public BasedOn BasedOn { get; set; }
    
    public string ClientId { get; set; }
    
    public DateTime? CompletedDate { get; set; }

    public bool IsCompleted => CompletedDate.HasValue;
    
    public Guid CityId { get; set; }
}

public class BasedOn
{
    public Guid Id { get; set; }
    
    public BasedOnType Type { get; set; }
}

public enum BasedOnType
{
    Reservation, AdHoc
}