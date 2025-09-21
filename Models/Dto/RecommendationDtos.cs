namespace FitnessBakcend.Models.Dto
{
    public class RecommendationRequest
    {
        public FitnessGoal? Goal { get; set; }
        public decimal? BudgetMin { get; set; }
        public decimal? BudgetMax { get; set; }
        public double Latitude { get; set; }   // user location
        public double Longitude { get; set; }
        public double MaxDistanceKm { get; set; } = 15; // search radius
        public int Take { get; set; } = 10;
    }

    public class RecommendedCoachDto
    {
        public int CoachId { get; set; }
        public string CoachName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;

        public double DistanceKm { get; set; }        // nearest gym distance
        public string GymName { get; set; } = string.Empty;
        public int GymId { get; set; }

        public decimal MinPlanPrice { get; set; }
        public double AvgRating { get; set; }
        public int ReviewsCount { get; set; }
        public IEnumerable<string> MatchingPlanTitles { get; set; } = Array.Empty<string>();
    }
}
