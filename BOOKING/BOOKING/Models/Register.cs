using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace BOOKING.Models
{
    public class Register
    {
        [Required(ErrorMessage = "Login jest wymagany!")]
        public string userName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "E-mail jest wymagany!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane!")]
        public string Password { get; set; }
    }
}
