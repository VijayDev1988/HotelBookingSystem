using HBS.Application.DTO.Room;
using HBS.Application.Features;
using HBS.Application.Interfaces;
using HBS.Application.Wrappers;
using HBS.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HBS.APIs.Controllers.v1
{
    [ApiVersion("1.0")]
    public class RoomController : BaseApiController
    {
        private readonly IRoomQuery _roomQuerys;

        public RoomController(IRoomQuery roomQuerys)
        {
            _roomQuerys = roomQuerys;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddRoomsAsync(RoomViewModel rooms)
        {
            var result = await _roomQuerys.AddRoomsAsync(rooms);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoomsAsync()
        {
            var result = await _roomQuerys.GetAllRoomsAsync();
            return Ok(result);
        }

        [HttpGet("RoomStatus")]
        [Authorize]
        public async Task<IActionResult> GetRoomsBookingStatusAsync(DateTime bookingDate)
        {
            var result = await _roomQuerys.GetRoomsBookingStatusAsync(bookingDate);
            return Ok(result);
        }
    }
}
