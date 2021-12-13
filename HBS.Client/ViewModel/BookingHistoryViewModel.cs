using HBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HBS.Client.ViewModel
{
    public class BookingHistoryViewModel
    {
        //public string RoomNumber { get; set; }
        //public RoomType RoomType { get; set; }
        //public DateTime BookingFromDate { get; set; }
        //public DateTime BookingToDate { get; set; }

        public int Id { get; set; }
        public RoomBooking RoomBooking { get; set; }
        public string UserEmail { get; set; }

        public string Message { get; set; }

    }
}
