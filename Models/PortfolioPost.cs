using System.ComponentModel.DataAnnotations;

namespace FitnessBakcend.Models
{
    public class PortfolioPost
    {
        public int Id { get; set; }
        public int CoachId { get; set; }
        public Coach Coach { get; set; } = default!;

        [Required, MaxLength(140)] public string Title { get; set; } = string.Empty;
        [MaxLength(1000)] public string Description { get; set; } = string.Empty;
        [MaxLength(500)] public string MediaUrl { get; set; } = string.Empty; // store file/url from your uploader
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
