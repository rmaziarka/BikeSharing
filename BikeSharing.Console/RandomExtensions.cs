using Microsoft.Azure.Cosmos.Spatial;

namespace BikeSharing.Console;


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

    public static Point GeneratePoint(this Random r, int cityIndex, double cityLat, double cityLong)
    {
        var longLatRadius = Constants.GetLongLatRadiusForCity(cityIndex);
        var stationLatitude = cityLat + r.NextGaussian(0, longLatRadius / 3);
        var stationLongitude = cityLong + r.NextGaussian(0, longLatRadius / 3);

        return new Point(stationLongitude, stationLatitude);
    }
}