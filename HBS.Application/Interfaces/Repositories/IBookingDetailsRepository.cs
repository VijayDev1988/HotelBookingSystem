using HBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBS.Application.Interfaces.Repositories
{
    public interface IBookingDetailsRepository : IGenericRepositoryAsync<BookingDetails>
    {
        public Task<IReadOnlyList<BookingDetails>> GetRoomsBookingByDateAsync(DateTime bookingTime);
        public Task<IReadOnlyList<BookingDetails>> GetRoomsBookingByDateAsync(DateTime fromBookingDate, DateTime toBookingDate, int roomId);
        public Task<IReadOnlyList<BookingDetails>> GetCustomerRoomsBookingDetails(string userEmailId);
    }
}
