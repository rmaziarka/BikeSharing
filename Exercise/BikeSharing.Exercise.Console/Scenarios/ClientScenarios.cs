using BikeSharing.Exercise.Shared.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace BikeSharing.Exercise.Console.Scenarios;

public static class ClientScenarios
{
    public static string Database = "database";

    public static string ClientsContainerName = "clients";
    
    public static async Task GetClientsInCity(CosmosClient cosmosClient, Guid cityId)
    {
        var clientsContainer = cosmosClient.GetContainer(Database, ClientsContainerName);

        var query = clientsContainer
            .GetItemLinqQueryable<Client>();

        var whereQuery = query
            .Where(el => el.CityId == cityId);
        
        FeedIterator<Client> queryResultSetIterator = whereQuery.ToFeedIterator();

        List<Client> clients = new List<Client>();

        var requestCharge = 0.0;
        while (queryResultSetIterator.HasMoreResults)
        {
            FeedResponse<Client> currentResultSet = await queryResultSetIterator.ReadNextAsync();
            foreach (Client client in currentResultSet)
            {
                clients.Add(client);
            }

            requestCharge += currentResultSet.RequestCharge;
        }
        
        System.Console.WriteLine($"[Q] GetClientsInCity - {clients.Count} Items, {requestCharge:F2} RU");
    }

    public static async Task AddClient(CosmosClient cosmosClient, Guid cityId, Guid clientId, string firstName, string secondName)
    {
        var clientsContainer = cosmosClient.GetContainer(Database, ClientsContainerName);
        
        var client = new Client()
        {
            Id = clientId,
            CityId = cityId,
            FirstName = firstName,
            SecondName = secondName
        };
        
        var createResponse = await clientsContainer.CreateItemAsync(client);
        
        System.Console.WriteLine($"[C] AddClient - {createResponse.RequestCharge:F2} RU");
    }
    
    public static async Task AddRentalToClient(CosmosClient cosmosClient, Guid cityId, Guid clientId, Guid rentalId, DateTime rentalStartDate)
    {
        var clientsContainer = cosmosClient.GetContainer(Database, ClientsContainerName);

        var clientResponse =
            await clientsContainer.ReadItemAsync<Client>(
                clientId.ToString(),
                new PartitionKey(cityId.ToString())
            );
        
        var client = clientResponse.Resource;

        var clientRental = new ClientRental()
        {
            Id = rentalId,
            StartDate = rentalStartDate
        };
        
        client.Rentals.Add(clientRental);
        
        var updateResponse = await clientsContainer.UpsertItemAsync(client);
        
        System.Console.WriteLine($"[C] AddRentalToClient - {updateResponse.RequestCharge:F2} RU");
    }
}