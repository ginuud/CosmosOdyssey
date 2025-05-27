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
                    .ThenInclude(ro => ro.From)
                    .Include(r => r.Routes)
                    .ThenInclude(ro => ro.To)
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
                foreach (var route in reservation.Routes)
                {
                    route.ReservationId = reservation.Id;
                    _context.RouteInfos.Update(route);
                }
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
        public async Task<List<Reservation>> GetAllAsync()
        {
            try
            {
                return await _context.Reservations
                    .Include(r => r.Routes)
                    .ThenInclude(ro => ro.From)
                    .Include(r => r.Routes)
                    .ThenInclude(ro => ro.To)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving all reservations from database: {ex.Message}");
                throw;
            }
        }
    }
}