using System.ComponentModel.DataAnnotations;

namespace PlatformaKursy.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Range(0, 100000)]
        public decimal Price { get; set; }
    }
}