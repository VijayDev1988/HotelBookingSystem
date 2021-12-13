using HBS.Application.DTO.Room;
using HBS.Application.Interfaces;
using HBS.Application.Interfaces.Repositories;
using HBS.Application.Wrappers;
using HBS.Domain.Entities;
using HBS.Persistence.Repositories;
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
    public class RoomBookingController : BaseApiController
    {

        private readonly IRoomQuery _roomQuerys;

        public RoomBookingController(IRoomQuery roomQuerys)
        {
            _roomQuerys = roomQuerys;
        }

        [HttpGet("{bookingdate}")]
        [Authorize]
        public async Task<IActionResult> GetRoomsBookingByDateAsync(DateTime bookingDate)
        {
            var result = await _roomQuerys.GetRoomsByDateAsync(bookingDate);
            return Ok(result);
        }

        [HttpPost("BookRoom")]
        [Authorize]
        public async Task<IActionResult> BookRoomAsync(RoomBookingViewModel roomBookingViewModel)
        {
            var result = await _roomQuerys.BookRoomAsync(roomBookingViewModel);
            //var result = await _roomQuerys.GetAvailableRoomsAsync(roomBookingViewModel);            
            return Ok(result);
        }

        [HttpGet("BookingHistory")]
        [Authorize]
        public async Task<IActionResult> GetCustomerRoomsBookingDetails(string mailId)
        {
            var result = await _roomQuerys.GetCustomerRoomsBookingDetails(mailId);
            return Ok(result);
        }
    }
}
