using Newtonsoft.Json;

namespace BikeSharing.Console.Case1.Models;

public class Rental: IClientId
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    public string Type => nameof(Rental);
    
    public Guid UserId { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime ExpirationDate { get; set; }
    
    public Guid BikeId { get; set; }
    
    public BasedOn BasedOn { get; set; }
    public string ClientId { get; set; }
    
    public DateTime? CompletedDate { get; set; }

    public bool IsCompleted => CompletedDate.HasValue;
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