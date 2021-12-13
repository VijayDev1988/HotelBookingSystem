using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HBS.Client.Models
{
    public class BookRoomModel
    {
        public int RoomId { get; init; }

        public int RoomType { get; init; }

        public string Status { get; init; }

        public DateTime FromTime { get; init; }

        public DateTime ToTime { get; init; }
        public string EmailId { get; init; }

    }
}
