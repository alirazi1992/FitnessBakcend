namespace FitnessBakcend.Models
{
    public class CoachGym
    {
        public int CoachId { get; set; }
        public Coach Coach { get; set; } = default!;

        public int GymId { get; set; }
        public Gym Gym { get; set; } = default!;

        // simple availability & price model (extend later if needed)
        public string AvailableDays { get; set; } = "Mon-Fri"; // e.g., "Mon,Wed,Fri"
        public TimeSpan StartTime { get; set; } = new(9, 0, 0);
        public TimeSpan EndTime { get; set; } = new(17, 0, 0);
        public decimal HourlyRate { get; set; } = 0m;
    }
}
