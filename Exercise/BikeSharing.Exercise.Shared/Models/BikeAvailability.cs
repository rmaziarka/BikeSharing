using Microsoft.Azure.Cosmos.Spatial;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BikeSharing.Exercise.Shared.Models;

public class BikeAvailability
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    public bool IsTaken => Owner != null;
    
    public Guid? StationId { get; set; }
    
    public Guid CityId { get; set; }
    
    public Point? Location { get; set; }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public BikeType BikeType { get; set; }
    
    public Owner? Owner { get; set; }
}

public class Owner
{
    public Guid Id { get; set; }
    
    public string ClientId { get; set; }
    
    public OwnerType Type { get; set; }
}

public enum OwnerType
{
    Rental, Reservation
}