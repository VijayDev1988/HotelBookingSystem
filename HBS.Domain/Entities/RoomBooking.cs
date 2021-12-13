using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBS.Domain.Entities
{
    public class RoomBooking : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Rooms Room { get; set; }

        public string Status { get; set; }

        public DateTime FromTime { get; set; }

        public DateTime ToTime { get; set; }


    }
}
