using System.ComponentModel.DataAnnotations;

namespace BOOKING.Models
{
    public class ImageUrls
    {
        [Key]
        public int Id { get; set; }
        
        [DataType(DataType.ImageUrl)]
        public string? Url { get; set; }
    }
}
