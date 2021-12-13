using HBS.Application.DTO.Room;
using HBS.Application.Wrappers;
using HBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBS.Application.Interfaces
{
    public interface IRoomQuery
    {
        public Task<Response<Rooms>> AddRoomsAsync(RoomViewModel rooms);
        public Task<Response<IEnumerable<Rooms>>> GetAllRoomsAsync();
        public Task<Response<IEnumerable<BookingDetails>>> GetRoomsByDateAsync(DateTime bookingDate);
        public Task<Response<BookingDetails>> BookRoomAsync(RoomBookingViewModel roomBookingViewModel);
        public Task<Response<IEnumerable<BookingDetails>>> GetCustomerRoomsBookingDetails(string userEmailId);

        //public Task<Response<IEnumerable<RoomBooking>>> GetAvailableRoomsAsync(RoomBookingViewModel roomBookingViewModel);
        public Task<Response<IEnumerable<Rooms>>> GetRoomsBookingStatusAsync(DateTime bookingDate);
    }
}
