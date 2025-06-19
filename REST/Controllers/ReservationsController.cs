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
                var reservedRoute = await repo.GetReservedRouteByRouteInfoIdsAsync(reservationDto.RouteInfoIds);

                if (reservedRoute == null)
                {
                    reservedRoute = await repo.CreateReservedRoute(reservationDto.RouteInfoIds);
                }

                var reservationModel = reservationDto.ToReservationFromCreate(reservedRoute.Id);
                var result = await repo.CreateAsync(reservationModel);

                return CreatedAtAction(nameof(GetReservation), new { id = reservationModel.Id }, reservationModel.ToReservationDto());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating reservation: {ex.Message}");
                return StatusCode(500, "An error occurred while creating the reservation.");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllReservations()
        {
            var reservations = await repo.GetAllAsync();
            var dto = reservations.Select(r => r.ToReservationDto());
            return Ok(dto);
        }
    }
}