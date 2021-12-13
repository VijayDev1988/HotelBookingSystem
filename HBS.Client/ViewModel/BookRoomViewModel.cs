using HBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HBS.Client.ViewModel
{
    public class BookRoomViewModel : IValidatableObject
    {
        [Required]
        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }
        [Required]
        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }

        [Display(Name = "Room Type")]
        public RoomType? RoomType { get; set; }

        [NotMapped]
        public string EmailId { get; set; }

        [NotMapped]
        public string RoomNumber { get; set; }

        [NotMapped]
        public Guid BookingId { get; set; }

        [NotMapped]
        public string Message { get; set; }

        [NotMapped]
        public string jwtToken { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.ToDate < this.FromDate)
            {
                yield return new ValidationResult(
                    errorMessage: "To Date must be greater than From Date",
                    memberNames: new[] { "ToDate" }
               );
            }

            if (this.FromDate.Date < DateTime.Now.Date)
            {
                {
                    yield return new ValidationResult(
                        errorMessage: "From Date must be greater than Current Date",
                        memberNames: new[] { "FromDate" }
                   );
                }
            }
        }
    }
}
