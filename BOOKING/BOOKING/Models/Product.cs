using System.ComponentModel.DataAnnotations;

namespace BOOKING.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Opis jest wymagany!")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Kategoria jest wymagana!")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Miasto jest wymagane!")]
        public string Locality { get; set; }

    }
}
