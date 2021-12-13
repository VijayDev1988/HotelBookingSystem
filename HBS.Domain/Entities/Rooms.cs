using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBS.Domain.Entities
{
    public class Rooms : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string RoomNumber { get; set; }

        public RoomType RoomType { get; set; }

        public string RoomDescription { get; set; }

        public bool IsActive { get; set; }

        public Hotel Hotel { get; set; }

        [NotMapped]
        public string Status { get; set; }
    }
}
