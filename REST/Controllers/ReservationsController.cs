using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CosmosOdyssey.REST.Data;
using CosmosOdyssey.REST.Models;
using CosmosOdyssey.REST.Dtos;
using REST.Interfaces;
using REST.Mappers;

namespace REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationRepository repo;

        public ReservationsController(IReservationRepository reservationsRepo)
        {
            repo = reservationsRepo;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservation([FromRoute] int id)
        {

            var reservation = await repo.GetByIdAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }
            return Ok(reservation.ToReservationDto());
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReservationDto reservationDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                // var routeInfoIds = reservationDto.Routes.Select(r => r.Id).ToList();
                // var routeInfos = await repo.GetRouteInfosByIdsAsync(routeInfoIds);
                var routeInfos = await repo.GetRouteInfosByIdsAsync(reservationDto.RouteIds);

                // if (routeInfos.Count != routeInfoIds.Count)
                // {
                //     return BadRequest("One or more RouteInfo IDs are invalid.");
                // }

                var reservationModel = reservationDto.ToReservationFromCreate(routeInfos);
                var result = await repo.CreateAsync(reservationModel);

                return CreatedAtAction(nameof(GetReservation), new { id = reservationModel.Id }, reservationModel.ToReservationDto());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating reservation: {ex.Message}");
                return StatusCode(500, "An error occurred while creating the reservation.");
            }
        }

    }
}