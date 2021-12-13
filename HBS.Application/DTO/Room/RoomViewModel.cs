using HBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBS.Application.DTO.Room
{
    public class RoomViewModel
    {
        public string RoomNumber { get; set; }
        public int RoomType { get; set; }
        public bool IsActive { get; set; }

        public int HotelId { get; set; }

        //public Hotel Hotel { get; set; }
    }
}
