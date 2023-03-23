using Newtonsoft.Json;

namespace BikeSharing.Exercise.Shared.Models;

public class Client
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string SecondName { get; set; }
    
    public Guid CityId { get; set; }
    
    public List<ClientRental> Rentals { get; set; } = new List<ClientRental>();

    public int RentalCount => Rentals.Count;
}

public class ClientRental
{
    public Guid Id { get; set; }
    
    public DateTime StartDate { get; set; }
}