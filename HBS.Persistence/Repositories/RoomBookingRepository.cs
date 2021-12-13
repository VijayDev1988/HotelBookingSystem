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
    public class RoomBookingRepositoryAsync : GenericRepositoryAsync<RoomBooking>, IRoomBookingRepository
    {

        private readonly DbSet<RoomBooking> _roomBookings;
        private readonly DbSet<Rooms> _rooms;

        public RoomBookingRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _roomBookings = dbContext.Set<RoomBooking>();
            _rooms = dbContext.Set<Rooms>();

        }

        public async Task<List<RoomBooking>> GetAvailableRoomsAsync(DateTime fromBookingDate, DateTime toBookingDate, int roomTypeId)
        {
            // Get the booking detilas for the given days
            var bookings = await _roomBookings
                .Include(r => r.Room)
                .Where(r => (fromBookingDate >= r.FromTime && fromBookingDate <= r.ToTime)
                                    || (toBookingDate >= r.FromTime && toBookingDate <= r.ToTime)
                                    || (r.FromTime >= fromBookingDate && r.FromTime <= toBookingDate)
                                    || (r.ToTime >= fromBookingDate && r.ToTime <= toBookingDate)
                                    ).ToListAsync();

            //Left join to get the available room for the days
            var result = (from room in _rooms.ToList()
                          join booking in bookings
                              on room.Id equals booking.Room.Id into book
                          from sub in book.DefaultIfEmpty()
                          where sub == null && room.RoomType.Equals((RoomType)roomTypeId)
                          select new RoomBooking { Room = room }).ToList();

            return result;
        }
    }
}
