using HBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBS.Application.Interfaces.Repositories
{
    public interface IRoomRepository : IGenericRepositoryAsync<Rooms>
    {
        public Task<Rooms> GetRoomsWithHotel(int roomId);
        public Task<List<Rooms>> GetRoomsBookingStatusAsync(DateTime bookingDate);
    }
}
