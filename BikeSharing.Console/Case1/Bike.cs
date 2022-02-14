using Microsoft.Azure.Cosmos.Spatial;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BikeSharing.Console.Case1;

public class Bike
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    
    public bool IsTaken { get; set; }
    
    public Guid? StationId { get; set; }
    
    public Guid CityId { get; set; }
    
    public Point? Location { get; set; }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public BikeType BikeType { get; set; }
    
    public Owner Owner { get; set; }
}

public class Owner
{
    public Guid Id { get; set; }
    
    public OwnerType Type { get; set; }
}

public enum OwnerType
{
    Rental, Reservation
}