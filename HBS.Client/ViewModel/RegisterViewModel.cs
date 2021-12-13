using HBS.Client.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace HBS.Client.ViewModel
{
    public class RegisterViewModel : IValidatableObject
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [MinLength(6)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6,ErrorMessage ="Password must be minimum 6 character length")]
        [DataType(DataType.Password,ErrorMessage ="Password must be a string type")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Validate for password
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Regex re = new Regex(ApplicationsConstants.PASSWORD_REGEX);            
            if (!re.IsMatch(this.Password))
            {
                yield return new ValidationResult(
                    errorMessage: "Password should have at least one letter and one number and may contain special characters",
                    memberNames: new[] { "Password" }
               );
            }
        }
    }
}
