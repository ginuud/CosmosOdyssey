// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using CosmosOdyssey.REST.Models;
// using CosmosOdyssey.REST.Data;
// using CosmosOdyssey.REST.Dtos;

// namespace REST.Mappers
// {
//     public static class ReservationMapper
//     {
//         public static Reservation ToReservationFromCreate(this CreateReservationDto reservationDto)
//         {
//             return new Reservation
//             {
//                 FirstName = reservationDto.FirstName,
//                 LastName = reservationDto.LastName,
//                 ProviderId = reservationDto.ProviderId
//             };
//         }

//         public static ReservationDto ToReservationDto(this Reservation reservationrModel)
//         {
//             return new ReservationDto
//             {
//                 Id = reservationrModel.Id,
//                 FirstName = reservationrModel.FirstName,
//                 LastName = reservationrModel.LastName,
//                 Routes = reservationrModel.Routes.Select(r => new RouteDto
//                 {
//                     Id = r.Id,
//                     FlightStart = r.FlightStart,
//                     FlightEnd = r.FlightEnd,
//                     Price = r.Price,
//                     LegId = r.LegId,
//                     CompanyName = reservationrModel.CompanyName
//                 }).ToList(),

//                 ProviderId = reservationrModel.ProviderId,

//             };
//         }
//     }
// }