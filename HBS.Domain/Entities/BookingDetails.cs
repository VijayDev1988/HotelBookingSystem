using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBS.Domain.Entities
{
    public class BookingDetails : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public RoomBooking RoomBooking { get; set; }

        [EmailAddress]
        public string UserEmail { get; set; }

    }
}
