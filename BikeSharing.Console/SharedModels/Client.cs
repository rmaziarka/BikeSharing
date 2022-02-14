using Microsoft.Azure.Cosmos.Spatial;

namespace BikeSharing.Console.SharedModels;

public class Client
{
    public Guid Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    // in actual system there wouldn't be such property
    public Guid CityId { get; set; }
}