using System.ComponentModel.DataAnnotations;

namespace FitnessBakcend.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int CoachId { get; set; }
        public Coach Coach { get; set; } = default!;

        public int ClientId { get; set; }
        public Client Client { get; set; } = default!;

        [Range(1, 5)] public int Rating { get; set; } = 5;
        [MaxLength(1000)] public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
