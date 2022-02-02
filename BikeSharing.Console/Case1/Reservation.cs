using Newtonsoft.Json;

namespace BikeSharing.Console.Case1;

public class Reservation
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime ExpirationDate { get; set; }
    
    public Guid BikeId { get; set; }
}