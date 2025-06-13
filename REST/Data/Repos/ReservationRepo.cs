using CosmosOdyssey.REST.Data;
using REST.Interfaces;
using CosmosOdyssey.REST.Models;
using Microsoft.EntityFrameworkCore;
using REST.Models;

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
                    .Include(r => r.ReservedRoute)
                        .ThenInclude(r => r.RouteSegments.OrderBy(s => s.SegmentOrder))
                                .ThenInclude(rs => rs.RouteInfo)
                                    .ThenInclude(ri => ri.From)
                        .Include(r => r.ReservedRoute)
                            .ThenInclude(r => r.RouteSegments)
                                .ThenInclude(rs => rs.RouteInfo)
                                    .ThenInclude(ri => ri.To)
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
        public async Task<ReservedRoute> CreateReservedRoute(List<Guid> routeInfoIds)
        {
            try
            {
                var reservedRoute = new ReservedRoute();

                for (int i = 0; i < routeInfoIds.Count; i++)
                {
                    reservedRoute.RouteSegments.Add(new RouteSegment
                    {
                        RouteInfoId = routeInfoIds[i],
                        SegmentOrder = i
                    });
                }

                await _context.ReservedRoutes.AddAsync(reservedRoute);
                await _context.SaveChangesAsync();
                return reservedRoute;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating reserved route: {ex.Message}");
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
        public async Task<ReservedRoute> GetReservedRouteByRouteInfoIdsAsync(List<Guid> routeInfoIds)
        {
            try
            {
                var candidateRoutes = await _context.ReservedRoutes
                    .Include(rr => rr.RouteSegments)
                    .Where(rr => rr.RouteSegments.Count == routeInfoIds.Count)
                    .ToListAsync();

                foreach (var route in candidateRoutes)
                {
                    var orderedIds = route.RouteSegments
                        .OrderBy(rs => rs.SegmentOrder)
                        .Select(rs => rs.RouteInfoId)
                        .ToList();

                    if (orderedIds.SequenceEqual(routeInfoIds))
                        return route;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving reserved route from database: {ex.Message}");
                throw;
            }
        }
        public async Task<List<Reservation>> GetAllAsync()
        {
            try
            {
                return await _context.Reservations
                    .Include(r => r.ReservedRoute)
                        .ThenInclude(r => r.RouteSegments.OrderBy(s => s.SegmentOrder))
                            .ThenInclude(rs => rs.RouteInfo)
                                .ThenInclude(ri => ri.From)
                    .Include(r => r.ReservedRoute)
                        .ThenInclude(rr => rr.RouteSegments)
                            .ThenInclude(rs => rs.RouteInfo)
                                .ThenInclude(ri => ri.To)
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