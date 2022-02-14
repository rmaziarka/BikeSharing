using BikeSharing.Console.SharedModels;
using Microsoft.Azure.Cosmos.Spatial;

namespace BikeSharing.Console.Shared;


public static class RandomExtensions
{
    /// <summary>
    ///   Generates normally distributed numbers. Each operation makes two Gaussians for the price of one, and apparently they can be cached or something for better performance, but who cares.
    /// </summary>
    /// <param name="r"></param>
    /// <param name = "mu">Mean of the distribution</param>
    /// <param name = "sigma">Standard deviation</param>
    /// <returns></returns>
    public static double NextGaussian(this Random r, double mu = 0, double sigma = 1)
    {
        var u1 = r.NextDouble();
        var u2 = r.NextDouble();

        var rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                              Math.Sin(2.0 * Math.PI * u2);

        var rand_normal = mu + sigma * rand_std_normal;

        return rand_normal;
    }

    public static Point GetNormalRandomPointInCity(this Random r, int cityIndex, double cityLat, double cityLong)
    {
        var longLatRadius = Constants.GetLongLatRadiusForCity(cityIndex);
        var stationLatitude = cityLat + r.NextGaussian(0, longLatRadius / 3);
        var stationLongitude = cityLong + r.NextGaussian(0, longLatRadius / 3);

        return new Point(stationLongitude, stationLatitude);
    }

    public static Bike GetRandomBike(this Random r, Guid cityId)
    {
        var bikes = StaticLists.CitiesDict[cityId].Bikes;

        var numberOfBikes = bikes.Count;

        var randomNumber = r.Next(0, numberOfBikes);

        return bikes[randomNumber];
    }

    public static DateTime GetRandomDateInDay(this Random r, DateOnly date)
    {
        var startDate = date.ToDateTime(new TimeOnly(0, 0));
        var endDate = startDate.AddDays(1);
        
        TimeSpan timeSpan = endDate - startDate;
        TimeSpan newSpan = new TimeSpan(0, r.Next(0, (int)timeSpan.TotalMinutes), 0);
        DateTime newDate = startDate + newSpan;

        return newDate;
    }
    
    public static String GetClientIdForRental(this Random r, int cityIndex, int rentalIndex)
    {
        var numberOfRentals = Constants.GetNumberOfRentalsForCity(cityIndex);

        var numberOfClients = Constants.GetNumberOfClientsForCity(cityIndex);

        var clientIndex = 0;

        // 1% of clients get 10% of rentals = 4 rentals per 1 client 
        var onePercentClients = numberOfClients / 100;
        var twentyPercentRentals = numberOfRentals / 5;
        if (rentalIndex < twentyPercentRentals)
        {
            clientIndex = rentalIndex % onePercentClients;
            return $"user-{cityIndex}-{clientIndex}";
        }

        // 10% of clients get 50% of rentals = 1 rental per client
        var tenPercentClients = numberOfClients / 10;
        var fiftyPercentRentals = numberOfRentals / 2;
        if (rentalIndex < (twentyPercentRentals + fiftyPercentRentals))
        {
            clientIndex = (rentalIndex-twentyPercentRentals) % tenPercentClients;
            return $"user-{cityIndex}-{clientIndex}";
        }
        
        // the rest clients gets rental by random
        clientIndex = r.Next(onePercentClients + tenPercentClients, numberOfClients);
        return $"user-{cityIndex}-{clientIndex}";
    }

}