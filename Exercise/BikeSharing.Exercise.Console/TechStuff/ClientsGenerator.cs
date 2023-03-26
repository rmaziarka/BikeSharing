using BikeSharing.Exercise.Shared.Models;
using Microsoft.Azure.Cosmos;

namespace BikeSharing.Exercise.Console.TechStuff;

public static class ClientsGenerator
{
    public static string Database = "database";

    public static string ClientsContainerName = "clients";
    public static async Task GenerateClients(CosmosClient cosmosClient)
    {
        var clientsContainer = cosmosClient.GetContainer(Database, ClientsContainerName);

        double totalClientsRUCharge = 0;

        List<Task> concurrentTasks = new List<Task>();
        foreach (var client in StaticLists.Clients)
        {
            concurrentTasks.Add(clientsContainer.CreateItemAsync(client,
                new PartitionKey(client.CityId.ToString())));
        }
        
        await Task.WhenAll(concurrentTasks);
        foreach (var task in concurrentTasks)
        {
            var response = ((Task<ItemResponse<Client>>)task).Result;
            totalClientsRUCharge += response.RequestCharge;
        }

        System.Console.WriteLine($"RU cost for adding all {StaticLists.Clients.Count} clients :{totalClientsRUCharge:F2}");
    }
}