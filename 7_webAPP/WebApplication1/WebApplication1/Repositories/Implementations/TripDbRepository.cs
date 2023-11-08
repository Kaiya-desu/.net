using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTO.Requests;
using WebApplication1.DTO.Responses;
using WebApplication1.Models;

namespace WebApplication1.Repositories.Implementations
{
    public class TripDbRepository : ITripDbRepository
    {
        private readonly _2019SBDContext _context;

        public TripDbRepository(_2019SBDContext context)
        {
            _context = context;
        }
        
        public async Task<ICollection<TripResponseDTO>> GetTripsFromDb()
        {
            List<TripResponseDTO> tripsList = new List<TripResponseDTO>();
            var tripsFromDb = await _context.Trips
                .Select(trip => new Trip
                    {
                        IdTrip = trip.IdTrip,
                        Name = trip.Name,
                        Description = trip.Description,
                        DateFrom = trip.DateFrom,
                        DateTo = trip.DateTo,
                        MaxPeople = trip.MaxPeople,
                    }
                ).OrderBy(x => x.DateFrom).ToArrayAsync();
            
            for (int i = 0; i < tripsFromDb.Length; i++)
            {
                var clientsFromDb = await _context.ClientTrips.Where(clientTrip => clientTrip.IdTrip == tripsFromDb[i].IdTrip)
                    .Include(clientTrip => clientTrip.IdClientNavigation)
                    .Select(c => new ClientResponseDTO
                        {FirstName = c.IdClientNavigation.FirstName, LastName = c.IdClientNavigation.LastName})
                    .ToArrayAsync();

                var countryFromDb = await _context.CountryTrips.Where(countryTrip => countryTrip.IdTrip == tripsFromDb[i].IdTrip)
                    .Include(contryTrip => contryTrip.IdCountryNavigation)
                    .Select(c => new CountryResponseDTO {Name = c.IdCountryNavigation.Name}).ToArrayAsync();

                tripsList.Add(new TripResponseDTO
                {
                    Name = tripsFromDb[i].Name,
                    Description = tripsFromDb[i].Description,
                    DateFrom = tripsFromDb[i].DateFrom,
                    DateTo = tripsFromDb[i].DateTo,
                    MaxPeople = tripsFromDb[i].MaxPeople,
                    Clients = clientsFromDb,
                    Countries = countryFromDb
                });
            }

            return tripsList;
        }

        public async Task<MyStatus> PostClientToTrip(int idTrip, ClientRequestDTO client)
        {
            var code = 0;
            var message = "";
            int idClient;

            // i
            // czy klient o danym numerze PESEL istnieje, jak nie dodaje go do bazy
            var clientInDb = await _context.Clients.AnyAsync(x => x.Pesel == client.Pesel);
            if (!clientInDb) {
                idClient = _context.Clients.Max(x => x.IdClient) + 1;
                await _context.Clients.AddAsync(new Client {
                    IdClient = idClient,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Email = client.Email,
                    Telephone = client.Telephone,
                    Pesel = client.Pesel
                });
            }
            else {
                var c = await _context.Clients.SingleOrDefaultAsync(x => x.Pesel == client.Pesel);
                idClient = c.IdClient;
            }
            
            // dodatkowa walidacja danych (na podstawie jsona z body) czy [FromRoute] IdTrip == [FromBody] IdTrip
            if (!ValidateIdTripRouteToIdTripBody(idTrip, client.IdTrip))
            {
                code = 404;
                message = "IdTrip from [Body] should be the same as IdTrip from [Route]!";
            }
            
            // iii
            // czy wycieczka istnieje? + dodatkowa walidacja danych (na podstawie jsona z body) - czy nazwa odpowiada idTrip?
            else if(! await ValidateTrip(idTrip))
            {
                code = 404;
                message = "Wrong IdTrip. There is no such trip in database!";
            }
            
            // dodatkowa walidacja danych (na podstawie jsona z body) - czy nazwa odpowiada id?
            else if (! await ValidateTripName(idTrip, client.TripName))
            {
                code = 404;
                message = "TripName does not equal IdTrip.Name from DB!";
            }
            
            // ii
            // czy nie jest zapisany na tą wycieczke?
            else if (await ValidateClientOnTrip(idClient, idTrip))
            {
                code = 404;
                message = "Client already in this trip";
            }
            
            else
            {
                await _context.ClientTrips.AddAsync(new ClientTrip
                {
                    IdClient = idClient,
                    IdTrip = idTrip,
                    RegisteredAt = DateTime.Now,
                    PaymentDate = client.PaymentDate,
                });
                await _context.SaveChangesAsync();
                
                code = 200;
                message = "Client added to the trip!";
            }

            var answer = new MyStatus
            {
                Code = code,
                Message = message
            };

            return answer;
        }

        
        private async Task<bool> ValidateTrip(int idTrip)
        {
            return await _context.Trips.AnyAsync(x => x.IdTrip == idTrip);
        }
        
        private async Task<bool> ValidateClientOnTrip(int idClient, int idTrip)
        {
            return await _context.ClientTrips.AnyAsync(x => x.IdClient == idClient && x.IdTrip == idTrip);
        }
        
        // dodatkowa walidacja danych (tj nie ma tego w tresci zadania ale wydaje sie logiczne c: )
        // czy IdTrip w Body jest taki sam jak podany w route?
        private bool ValidateIdTripRouteToIdTripBody(int idTripRoute, int idTripBody)
        {
            return idTripRoute == idTripBody;
        }

        // czy name podany w body odpowiada idTrip?
        private async Task<bool> ValidateTripName(int idTrip, string tripName)
        {
            return await _context.Trips.AnyAsync(x => x.IdTrip == idTrip && x.Name == tripName);
        }

        
    }
}