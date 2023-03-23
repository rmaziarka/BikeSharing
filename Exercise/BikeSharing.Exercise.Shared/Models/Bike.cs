using Newtonsoft.Json;

namespace BikeSharing.Exercise.Shared.Models;

public class Bike
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public Guid CityId { get; set; }
}
