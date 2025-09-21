using System.ComponentModel.DataAnnotations;

namespace FitnessBakcend.Models
{
    public class Coach
    {
        public int Id { get; set; }

        [Required, MaxLength(100)] public string Name { get; set; } = string.Empty;
        [Required, MaxLength(100)] public string Specialization { get; set; } = string.Empty;

        [MaxLength(2000)] public string Bio { get; set; } = string.Empty;
        public int YearsExperience { get; set; }
        [MaxLength(500)] public string Certifications { get; set; } = string.Empty;

        public ICollection<CoachGym> CoachGyms { get; set; } = new List<CoachGym>();
        public ICollection<TrainingPlan> Plans { get; set; } = new List<TrainingPlan>();
        public ICollection<PortfolioPost> Portfolio { get; set; } = new List<PortfolioPost>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
