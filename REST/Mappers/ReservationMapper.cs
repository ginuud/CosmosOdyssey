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
        public static Reservation ToReservationFromCreate(this CreateReservationDto reservationDto)
        {
            return new Reservation
            {
                FirstName = reservationDto.FirstName,
                LastName = reservationDto.LastName,
                ProviderId = reservationDto.ProviderId
            };
        }

        public static ReservationDto ToReservationDto(this Reservation reservationrModel)
        {
            return new ReservationDto
            {
                Id = reservationrModel.Id,
                FirstName = reservationrModel.FirstName,
                LastName = reservationrModel.LastName,
                ProviderId = reservationrModel.ProviderId

            };
        }
    }
}