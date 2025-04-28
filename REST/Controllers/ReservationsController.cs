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


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReservationDto reservationDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var reservationModel = reservationDto.ToReservationFromCreate();
            await repo.CreateAsync(reservationModel);

            return CreatedAtAction(nameof(Create), new { reservationModel.Id }, reservationModel.ToReservationDto());
        }

    }
}