using System.ComponentModel.DataAnnotations;

namespace FitnessBakcend.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required, MaxLength(100)] public string Name { get; set; } = string.Empty;
        [MaxLength(200), EmailAddress] public string Email { get; set; } = string.Empty;
        [MaxLength(30)] public string PhoneNumber { get; set; } = string.Empty;

        // Preferences for recommendations
        public FitnessGoal PreferredGoal { get; set; } = FitnessGoal.GeneralFitness;
        public decimal BudgetMin { get; set; }
        public decimal BudgetMax { get; set; }

        // For “nearest gym”
        public double HomeLatitude { get; set; }
        public double HomeLongitude { get; set; }

        [MaxLength(300)] public string Address { get; set; } = string.Empty;
        [MaxLength(100)] public string PreferredGym { get; set; } = string.Empty;
        [MaxLength(1000)] public string Goals { get; set; } = string.Empty;
    }
}
