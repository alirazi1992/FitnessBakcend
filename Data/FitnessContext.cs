using Microsoft.EntityFrameworkCore;
using FitnessBakcend.Models;

namespace FitnessBakcend.Data
{
    public class FitnessContext : DbContext
    {
        public FitnessContext(DbContextOptions<FitnessContext> options) : base(options) { }

        public DbSet<Coach> Coaches => Set<Coach>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Gym> Gyms => Set<Gym>();
        public DbSet<CoachGym> CoachGyms => Set<CoachGym>();
        public DbSet<TrainingPlan> TrainingPlans => Set<TrainingPlan>();
        public DbSet<PortfolioPost> PortfolioPosts => Set<PortfolioPost>();
        public DbSet<Review> Reviews => Set<Review>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CoachGym>().HasKey(x => new { x.CoachId, x.GymId });
            modelBuilder.Entity<CoachGym>()
                .HasOne(x => x.Coach).WithMany(c => c.CoachGyms).HasForeignKey(x => x.CoachId);
            modelBuilder.Entity<CoachGym>()
                .HasOne(x => x.Gym).WithMany(g => g.CoachGyms).HasForeignKey(x => x.GymId);

            modelBuilder.Entity<TrainingPlan>()
                .HasOne(p => p.Coach).WithMany(c => c.Plans).HasForeignKey(p => p.CoachId);

            modelBuilder.Entity<PortfolioPost>()
                .HasOne(p => p.Coach).WithMany(c => c.Portfolio).HasForeignKey(p => p.CoachId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Coach).WithMany(c => c.Reviews).HasForeignKey(r => r.CoachId);
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Client).WithMany().HasForeignKey(r => r.ClientId);

            modelBuilder.Entity<TrainingPlan>().Property(p => p.Price).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<CoachGym>().Property(p => p.HourlyRate).HasColumnType("decimal(10,2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
