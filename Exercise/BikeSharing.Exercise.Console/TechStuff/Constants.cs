namespace BikeSharing.Exercise.Console.TechStuff;

public static class Constants
{
    public const int NumberOfCities = 100;

    public const int MaxNumberOfStations = 400;

    public const int MaxNumberOfBikes = 4000;

    public const int MaxNumberOfRentals = 40000;

    public const int MaxNumberOfClients = 200000;
    
    // 0.1 long&lat = 11km = +/- warsaw radius
    public const double MaxLongLatRadius = 0.1;
    
    // to create linear distribution between 400X and X between 100 elements
    public const double LoweringDiscriminator = 0.0097;

    public static double GetRatioForCity(int cityIndex) => 1 - (cityIndex * LoweringDiscriminator);

    public static int GetNumberOfBikesForCity(int cityIndex) => (int) (GetRatioForCity(cityIndex) * MaxNumberOfBikes);
    
    public static int GetNumberOfRentalsForCity(int cityIndex) => (int) (GetRatioForCity(cityIndex) * MaxNumberOfRentals);
    
    public static int GetNumberOfClientsForCity(int cityIndex) => (int) (GetRatioForCity(cityIndex) * MaxNumberOfClients);
    
    public static double GetLongLatRadiusForCity(int cityIndex) => GetRatioForCity(cityIndex) * MaxLongLatRadius;
}