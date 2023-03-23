﻿using Microsoft.Azure.Cosmos.Spatial;

namespace BikeSharing.Exercise.Shared.Models;

public class Station
{
    public Guid Id { get; set; }
    
    public Guid CityId { get; set; }
    
    public Point Location { get; set; }
    
    public string Name { get; set; }
}