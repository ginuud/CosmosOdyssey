using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosOdyssey.REST.Models;
using CosmosOdyssey.REST.Dtos;
using REST.Models;

namespace REST.Interfaces
{
    public interface IReservationRepository
    {
        Task<Reservation> CreateAsync(Reservation reservationModel);
        Task<List<RouteInfo>> GetRouteInfosByIdsAsync(List<Guid> routeInfoIds);
        Task<ReservedRoute> GetReservedRouteByRouteInfoIdsAsync(List<Guid> routeInfoIds);
        Task<ReservedRoute> CreateReservedRoute(List<Guid> routeInfoIds);
        Task<Reservation?> GetByIdAsync(int id);
        Task<List<Reservation>> GetAllAsync();
    }
}