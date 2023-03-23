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
        foreach (var clients in StaticLists.Clients.BatchBy(20))
        {
            List<Task> concurrentTasks = new List<Task>();
            foreach (var client in clients)
            {
                concurrentTasks.Add(clientsContainer.CreateItemAsync(client,
                    new PartitionKey(client.CityId.ToString())));

                await Task.WhenAll(concurrentTasks);

                double clientsRUCharge = 0;
                foreach (var task in concurrentTasks)
                {
                    var response = ((Task<ItemResponse<BikeAvailability>>)task).Result;
                    clientsRUCharge += response.RequestCharge;
                }

                totalClientsRUCharge += clientsRUCharge;
                System.Console.WriteLine($"RU cost for adding 20 clients:{clientsRUCharge:F2}");
                System.Console.WriteLine($"Cost per bike {(clientsRUCharge / 20):F2}");
            }
        }

        System.Console.WriteLine($"RU cost for adding all {StaticLists.Clients.Count} clients :{totalClientsRUCharge:F2}");
    }
}