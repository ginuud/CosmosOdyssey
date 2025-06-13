using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosOdyssey.REST.Models;
using CosmosOdyssey.REST.Data;
using CosmosOdyssey.REST.Dtos;
using REST.Models;

namespace REST.Mappers
{
    public static class ReservationMapper
    {
        public static Reservation ToReservationFromCreate(this CreateReservationDto reservationDto, int RouteId)
        {
            return new Reservation
            {
                FirstName = reservationDto.FirstName,
                LastName = reservationDto.LastName,
                ReservedRouteId = RouteId,
                TotalQuotedPrice = reservationDto.TotalQuotedPrice,
                TotalQuotedTravelTime = reservationDto.TotalQuotedTravelTime,
                TransportationCompanyNames = reservationDto.TransportationCompanyNames ?? new List<string>(),
            };
        }

        public static ReservationDto ToReservationDto(this Reservation reservationModel)
        {
            var orderedSegments = reservationModel.ReservedRoute?.RouteSegments?
                .OrderBy(s => s.SegmentOrder)
                .ToList() ?? new List<RouteSegment>();


            return new ReservationDto
            {
                Id = reservationModel.Id,
                FirstName = reservationModel.FirstName,
                LastName = reservationModel.LastName,
                Routes = orderedSegments.Select(s => s.RouteInfo?.ToRouteInfoDto() ?? new RouteInfoDto()).ToList(),
                TotalQuotedPrice = reservationModel.TotalQuotedPrice,
                TotalQuotedTravelTime = reservationModel.TotalQuotedTravelTime,
                TransportationCompanyNames = reservationModel.TransportationCompanyNames,

            };
        }
    }
}