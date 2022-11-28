using System.ComponentModel.DataAnnotations;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace BOOKING.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Login jest wymagany!")]
        public string userName { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane!")]
        public string Password { get; set; }
    }
}
