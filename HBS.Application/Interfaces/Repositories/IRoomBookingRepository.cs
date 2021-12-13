using HBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBS.Application.Interfaces.Repositories
{
    public interface IRoomBookingRepository : IGenericRepositoryAsync<RoomBooking>
    {

        //public Task<IReadOnlyList<RoomBooking>> GetAvailableRoomsAsync(DateTime fromBookingDate, DateTime toBookingDate);

        public Task<List<RoomBooking>> GetAvailableRoomsAsync(DateTime fromBookingDate, DateTime toBookingDate, int roomTypeId);


    }
}
