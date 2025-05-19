using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosOdyssey.REST.Models;
using CosmosOdyssey.REST.Data;
using CosmosOdyssey.REST.Dtos;

namespace REST.Mappers
{
    public static class ReservationMapper
    {
        public static Reservation ToReservationFromCreate(this CreateReservationDto reservationDto, List<RouteInfo> routeInfos)
        {
            return new Reservation
            {
                FirstName = reservationDto.FirstName,
                LastName = reservationDto.LastName,
                Routes = routeInfos,
                RouteInfoIds = reservationDto.RouteIds ?? new List<Guid>(),
                TotalQuotedPrice = reservationDto.TotalQuotedPrice,
                TotalQuotedTravelTime = reservationDto.TotalQuotedTravelTime,
                TransportationCompanyNames = reservationDto.TransportationCompanyNames ?? new List<string>(),
            };
        }

        public static ReservationDto ToReservationDto(this Reservation reservationModel)
        {
            return new ReservationDto
            {
                Id = reservationModel.Id,
                FirstName = reservationModel.FirstName,
                LastName = reservationModel.LastName,
                Routes = reservationModel.Routes.Select(r => r.ToRouteInfoDto()).ToList(),
                TotalQuotedPrice = reservationModel.TotalQuotedPrice,
                TotalQuotedTravelTime = reservationModel.TotalQuotedTravelTime,
                TransportationCompanyNames = reservationModel.TransportationCompanyNames,

            };
        }
    }
}