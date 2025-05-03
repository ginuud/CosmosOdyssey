using CosmosOdyssey.REST.Data;
using REST.Interfaces;
using CosmosOdyssey.REST.Models;
using Microsoft.EntityFrameworkCore;

namespace REST.Data.Repos
{
    public class ReservationRepo(DataContext context) : IReservationRepository
    {
        private readonly DataContext _context = context;

        public async Task<Reservation> CreateAsync(Reservation reservation)
        {
            try
            {
                await _context.Reservations.AddAsync(reservation);
                await _context.SaveChangesAsync();
                return reservation;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"Error saving reservation to database: {ex.Message}");
                throw;
            }
        }
    }
}