using FitnessBakcend.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessBakcend.Data
{
    public static class Seed
    {
        public static async Task SeedDevDataAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<FitnessContext>();
            await db.Database.MigrateAsync();

            if (await db.Coaches.AnyAsync()) return; // already seeded

            var gyms = new[]
            {
                new Gym { Name="Central Fit", Location="Downtown", Latitude=51.5074, Longitude=-0.1278, PhoneNumber="020-1000" },
                new Gym { Name="North Power", Location="North Side", Latitude=51.5600, Longitude=-0.1500, PhoneNumber="020-2000" }
            };
            db.Gyms.AddRange(gyms);

            var coaches = new[]
            {
                new Coach { Name="Alex Turner", Specialization="Strength", YearsExperience=7, Bio="Power & hypertrophy", Certifications="NASM" },
                new Coach { Name="Mina K.", Specialization="Weight Loss", YearsExperience=5, Bio="Sustainable fat loss", Certifications="ACE" }
            };
            db.Coaches.AddRange(coaches);
            await db.SaveChangesAsync();

            db.CoachGyms.AddRange(
                new CoachGym { CoachId = coaches[0].Id, GymId = gyms[0].Id, AvailableDays = "Mon,Wed,Fri", StartTime = new(9, 0, 0), EndTime = new(17, 0, 0), HourlyRate = 50 },
                new CoachGym { CoachId = coaches[0].Id, GymId = gyms[1].Id, AvailableDays = "Tue,Thu", StartTime = new(10, 0, 0), EndTime = new(18, 0, 0), HourlyRate = 55 },
                new CoachGym { CoachId = coaches[1].Id, GymId = gyms[0].Id, AvailableDays = "Mon-Fri", StartTime = new(8, 0, 0), EndTime = new(16, 0, 0), HourlyRate = 40 }
            );

            db.TrainingPlans.AddRange(
                new TrainingPlan { CoachId = coaches[0].Id, Title = "Mass Builder 8w", Goal = FitnessGoal.MuscleGain, DurationWeeks = 8, Price = 199, Description = "Push/Pull/Legs + nutrition" },
                new TrainingPlan { CoachId = coaches[1].Id, Title = "Cut Smart 6w", Goal = FitnessGoal.WeightLoss, DurationWeeks = 6, Price = 149, Description = "HIIT + macro guide" }
            );

            db.PortfolioPosts.AddRange(
                new PortfolioPost { CoachId = coaches[0].Id, Title = "Client PR 180kg DL", Description = "Great progress!", MediaUrl = "https://example.com/dl.mp4" },
                new PortfolioPost { CoachId = coaches[1].Id, Title = "10kg down in 8 weeks", Description = "Consistency wins", MediaUrl = "https://example.com/weightloss.jpg" }
            );

            var client = new Client { Name = "Test Client", Email = "test@client.com", HomeLatitude = 51.509, HomeLongitude = -0.13, BudgetMin = 30, BudgetMax = 60 };
            db.Clients.Add(client);

            db.Reviews.AddRange(
                new Review { CoachId = coaches[0].Id, ClientId = client.Id, Rating = 5, Comment = "Amazing strength gains!" },
                new Review { CoachId = coaches[1].Id, ClientId = client.Id, Rating = 4, Comment = "Great fat-loss plan." }
            );

            await db.SaveChangesAsync();
        }
    }
}
