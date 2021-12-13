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
    public class BookingDetailsRepository : GenericRepositoryAsync<BookingDetails>, IBookingDetailsRepository
    {
        private readonly DbSet<BookingDetails> _bookingDetails;

        public BookingDetailsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _bookingDetails = dbContext.Set<BookingDetails>();
        }

        public async Task<IReadOnlyList<BookingDetails>> GetRoomsBookingByDateAsync(DateTime bookingTime)
        {
            return await _bookingDetails
                .Include(r => r.RoomBooking).Where(r => r.RoomBooking.FromTime >= bookingTime && r.RoomBooking.ToTime <= bookingTime)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<BookingDetails>> GetCustomerRoomsBookingDetails(string userEmailId)
        {
            return await _bookingDetails
                .Include(r => r.RoomBooking)
                    .ThenInclude(r => r.Room).Where(r => r.UserEmail.Equals(userEmailId))
                .ToListAsync();
        }

        public async Task<IReadOnlyList<BookingDetails>> GetRoomsBookingByDateAsync(DateTime fromBookingDate, DateTime toBookingDate, int roomId)
        {
            return await _bookingDetails
                .Include(r => r.RoomBooking)
                    .ThenInclude(r => r.Room)
                                    .Where(r => (fromBookingDate >= r.RoomBooking.FromTime && fromBookingDate <= r.RoomBooking.ToTime)
                                    || (toBookingDate >= r.RoomBooking.FromTime && toBookingDate <= r.RoomBooking.ToTime)
                                    || (r.RoomBooking.FromTime >= fromBookingDate && r.RoomBooking.FromTime <= toBookingDate)
                                    || (r.RoomBooking.ToTime >= fromBookingDate && r.RoomBooking.ToTime <= toBookingDate)
                                    && r.RoomBooking.Room.Id.Equals(roomId)
                                    ).ToListAsync();
        }
    }
}
