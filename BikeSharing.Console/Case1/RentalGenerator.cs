using BikeSharing.Console.Case1.Models;
using BikeSharing.Console.Shared;
using Microsoft.Azure.Cosmos;

namespace BikeSharing.Console.Case1;
using static System.Linq.Enumerable;
public static class RentalGenerator
{
    public static string Database = "case1";
    public static string RentalContainerName = "rentals";
    
    public static async Task GenerateReservationsAndRentals(CosmosClient cosmosClient, DateOnly generationDay, bool allCities = false)
    {
        var rentalsContainer = cosmosClient.GetContainer(Database, RentalContainerName);

        var random = new Random();
        var rentalReservationRU = 0.0;
        for (int cityIndex = 0; cityIndex < StaticLists.Cities.Count; cityIndex++)
        {
            var city = StaticLists.Cities[cityIndex];

            var numberOfRentals = Constants.GetNumberOfRentalsForCity(cityIndex);

            var reservationsAndRentals = new List<IClientId>();
            foreach (var rentalIndex in Range(0,numberOfRentals))
            {
                var clientId = random.GetClientIdForRental(cityIndex, rentalIndex);
                
                var basedOnType = rentalIndex % 2 == 0
                    ? BasedOnType.Reservation
                    : BasedOnType.AdHoc;

                if (basedOnType == BasedOnType.Reservation)
                {
                    var reservationStartDate = random.GetRandomDateInDay(generationDay);
                    var expirationDate = reservationStartDate.AddMinutes(15);
                        
                    var reservation = new Reservation()
                    {
                        Id = Guid.NewGuid(),
                        BikeId = random.GetRandomBike(city.Id).Id,
                        StartDate = reservationStartDate,
                        ExpirationDate = expirationDate,
                        ClientId = clientId,
                        CityId = city.Id
                    };

                    var rentalStartDate = reservationStartDate.AddMinutes(5);
                    var rentalExpirationDate = rentalStartDate.AddDays(1);
                    var rentalCompletedDate = rentalStartDate.AddMinutes(35);

                    var rental = new Rental()
                    {
                        Id = Guid.NewGuid(),
                        BikeId = reservation.BikeId,
                        ClientId = clientId,
                        StartDate = rentalStartDate,
                        ExpirationDate = rentalExpirationDate,
                        CompletedDate = rentalCompletedDate,
                        BasedOn = new BasedOn()
                        {
                            Id = reservation.Id,
                            Type = BasedOnType.Reservation
                        },
                        CityId = city.Id
                    };

                    reservation.Completed = new Completed()
                    {
                        Date = rental.StartDate,
                        RentalId = rental.Id
                    };
                    
                    reservationsAndRentals.Add(reservation);
                    reservationsAndRentals.Add(rental);
                }
                else
                {
                    var rentalStartDate = random.GetRandomDateInDay(generationDay);
                    var rentalExpirationDate = rentalStartDate.AddDays(1);
                    var rentalCompletedDate = rentalStartDate.AddMinutes(35);

                    var rental = new Rental()
                    {
                        Id = Guid.NewGuid(),
                        BikeId = random.GetRandomBike(city.Id).Id,
                        ClientId = clientId,
                        CityId = city.Id,
                        StartDate = rentalStartDate,
                        ExpirationDate = rentalExpirationDate,
                        CompletedDate = rentalCompletedDate  
                    };
                    
                    reservationsAndRentals.Add(rental);
                }
            }

            var cityRUCharge = 0.0;
            
            foreach(var reservationOrRentalBatch in reservationsAndRentals.BatchBy(1000))
            {
                List<Task> concurrentTasks = new List<Task>();
                foreach (var reservationOrRental in reservationOrRentalBatch)
                {
                    concurrentTasks.Add(rentalsContainer.CreateItemAsync<dynamic>(reservationOrRental,
                        new PartitionKey(reservationOrRental.ClientId)));
                }

                await Task.WhenAll(concurrentTasks);
            
                foreach (var task in concurrentTasks)
                {
                    var response = ((Task<ItemResponse<dynamic>>)task).Result;
                    cityRUCharge += response.RequestCharge;
                }
            }

            rentalReservationRU += cityRUCharge;
            System.Console.WriteLine($"RU cost for adding rentals to {city.Name} :{cityRUCharge:F2}");
            System.Console.WriteLine($"Average cost per rental {(cityRUCharge/ (double)numberOfRentals):F2}");

            if (!allCities) return;
        }
        System.Console.WriteLine($"RU cost for adding all bikes to {StaticLists.Cities.Count} cities :{rentalReservationRU:F2}");
    }
}