using System.ComponentModel;
using Microsoft.Azure.Cosmos.Spatial;

public class City
{
    public Guid Id { get; set; }
    
    public int Population { get; set; }
    
    public Point Location { get; set; }

    public string Name { get; set; }

    public List<Station> Stations { get; set; } = new List<Station>();
}