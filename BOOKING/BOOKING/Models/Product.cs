using BOOKING.CutomValidationAttributes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Required(ErrorMessage = "Początkowa data jest wymagana!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime startDate { get; set; }

        [Required(ErrorMessage = "Końcowa data jest wymagana!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime endDate { get; set; }

        public List<ImageUrls>? ImageUrls { get; set; } = new();

        [MaxFileSize(1 * 1024 * 1024)]
        [PermittedExtensions(new string[] { ".jpg", ".png", ".jpeg" })]
        [NotMapped]
        public virtual IFormFile[]? ImageFiles { get; set; }
        public List<ImageStorageNames>? ImageStorageNames { get; set; } = new();
    }
}
