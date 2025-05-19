using CosmosOdyssey.REST.Data;
using REST.Interfaces;
using CosmosOdyssey.REST.Models;
using Microsoft.EntityFrameworkCore;

namespace REST.Data.Repos
{
    public class ReservationRepo(DataContext context) : IReservationRepository
    {
        private readonly DataContext _context = context;

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            try
            {
                var dbReservation = await _context.Reservations
                    .Include(r => r.Routes)
                    .FirstOrDefaultAsync(r => r.Id == id);

                return dbReservation;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving reservation from database: {ex.Message}");
                throw;
            }
        }

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
                Console.Error.WriteLine($"Error saving reservation to database: {ex.Message}");
                throw;
            }
        }
        public async Task<List<RouteInfo>> GetRouteInfosByIdsAsync(List<Guid> routeInfoIds)
        {
            try
            {
                return await _context.RouteInfos
                    .Where(rI => routeInfoIds.Contains(rI.Id))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving route info from database: {ex.Message}");
                throw;
            }
        }
    }
}