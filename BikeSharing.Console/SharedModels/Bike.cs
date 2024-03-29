﻿using BikeSharing.Console.Case1;
using Microsoft.Azure.Cosmos.Spatial;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BikeSharing.Console.SharedModels;

public class Bike
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public Guid CityId { get; set; }
}
