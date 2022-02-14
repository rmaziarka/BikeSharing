using Newtonsoft.Json;

namespace BikeSharing.Console.Case1.Models;

public class Reservation: IClientId
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    public string Type => nameof(Reservation);
    
    public Guid UserId { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime ExpirationDate { get; set; }
    
    public Guid BikeId { get; set; }
    
    public Completed? Completed { get; set; }
    
    public string ClientId { get; set; }

    public bool IsCompleted => Completed != null;
}

public class Completed
{
    public DateTime Date { get; set; }
    
    public Guid RentalId { get; set; }
}