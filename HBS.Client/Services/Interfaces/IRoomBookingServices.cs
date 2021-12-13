using HBS.Application.Wrappers;
using HBS.Client.ViewModel;
using HBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HBS.Client.Services.Interfaces
{
    public interface IRoomBookingServices
    {
        Task<IEnumerable<BookingHistoryViewModel>> GetBookingHistory(string userEmail, string jwtToken);
        Task<BookRoomViewModel> BookingRoom(BookRoomViewModel bookRoomViewModel);
    }
}
