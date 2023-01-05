using System.ComponentModel.DataAnnotations;

namespace BOOKING.Models
{
    public class ImageStorageNames
    {
        [Key]
        public int Id { get; set; }
        
        public string? Name { get; set; }
    }
}
