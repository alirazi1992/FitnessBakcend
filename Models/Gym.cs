using System.ComponentModel.DataAnnotations;

namespace FitnessBakcend.Models
{
    public class Gym
    {
        public int Id { get; set; }

        [Required, MaxLength(120)] public string Name { get; set; } = string.Empty;
        [Required, MaxLength(200)] public string Location { get; set; } = string.Empty; // text address
        public double Latitude { get; set; }   // for distance calc
        public double Longitude { get; set; }

        [MaxLength(30)] public string PhoneNumber { get; set; } = string.Empty;

        public ICollection<CoachGym> CoachGyms { get; set; } = new List<CoachGym>();
    }
}
