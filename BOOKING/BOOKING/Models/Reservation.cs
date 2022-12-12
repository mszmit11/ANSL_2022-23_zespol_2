using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace BOOKING.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Imie jest wymagane!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane!")]
        public string secondName { get; set; }

        [Required(ErrorMessage = "Zameldowanie jest wymagane!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "Wymeldowanie jest wymagane!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime End { get; set; }
        [JsonIgnore]
        public string? CustomerId { get; set; }

        public int ProductId { get; set; }
    }
}
