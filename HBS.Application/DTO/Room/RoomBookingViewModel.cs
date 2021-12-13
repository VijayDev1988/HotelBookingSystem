using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBS.Application.DTO.Room
{
    public class RoomBookingViewModel
    {
        public int RoomId { get; set; }

        public int RoomType { get; set; }

        public string Status { get; set; }

        public DateTime FromTime { get; set; }

        public DateTime ToTime { get; set; }
        public string EmailId { get; set; }
    }
}
