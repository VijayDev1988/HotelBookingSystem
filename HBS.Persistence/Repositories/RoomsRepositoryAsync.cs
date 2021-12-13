using HBS.Application.Interfaces.Repositories;
using HBS.Domain.Entities;
using HBS.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBS.Persistence.Repositories
{
    public class RoomsRepositoryAsync : GenericRepositoryAsync<Rooms>, IRoomRepository
    {
        private readonly DbSet<Rooms> _rooms;
        private readonly DbSet<RoomBooking> _roomBookings;


        public RoomsRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _rooms = dbContext.Set<Rooms>();
            _roomBookings = dbContext.Set<RoomBooking>();
        }

        public async Task<Rooms> GetRoomsWithHotel(int roomId)
        {
            return _rooms.Include(h => h.Hotel).FirstOrDefault();
        }
               
        public async Task<List<Rooms>> GetRoomsBookingStatusAsync(DateTime bookingDate)
        {
            var bookings = await _roomBookings
                .Include(r => r.Room)
                .Where(r => (bookingDate >= r.FromTime && bookingDate <= r.ToTime)).ToListAsync();

            //Left join to get the status of the days
            var result = (from room in _rooms.ToList()
                          join booking in bookings
                              on room.Id equals booking.Room.Id into book
                          from sub in book.DefaultIfEmpty()
                          select new Rooms
                          {
                              Id = room.Id,
                              Hotel = room.Hotel,
                              RoomType = room.RoomType,
                              RoomNumber = room.RoomNumber,
                              Status = sub == null ? "Available" : "Booked"
                          }).ToList();

            return result;
        }
    }
}
