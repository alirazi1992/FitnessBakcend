using System.ComponentModel.DataAnnotations;

namespace FitnessBakcend.Models
{
    public class TrainingPlan
    {
        public int Id { get; set; }
        public int CoachId { get; set; }
        public Coach Coach { get; set; } = default!;

        [Required, MaxLength(120)] public string Title { get; set; } = string.Empty;
        public FitnessGoal Goal { get; set; } = FitnessGoal.GeneralFitness;
        public int DurationWeeks { get; set; } = 4;
        public decimal Price { get; set; } = 0m;

        [MaxLength(2000)] public string Description { get; set; } = string.Empty;
    }
}
