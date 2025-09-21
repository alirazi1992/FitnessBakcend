using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessBakcend.Data;
using FitnessBakcend.Models;
using FitnessBakcend.Models.Dto;

namespace FitnessBakcend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationsController : ControllerBase
    {
        private readonly FitnessContext _db;
        public RecommendationsController(FitnessContext db) => _db = db;

        [HttpPost]
        public async Task<ActionResult<IEnumerable<RecommendedCoachDto>>> Recommend([FromBody] RecommendationRequest req)
        {
            // 1) Pull minimal data needed
            var coaches = await _db.Coaches
                .Include(c => c.Plans)
                .Include(c => c.Reviews)
                .Include(c => c.CoachGyms).ThenInclude(cg => cg.Gym)
                .AsNoTracking()
                .ToListAsync();

            if (!coaches.Any()) return Ok(Array.Empty<RecommendedCoachDto>());

            // 2) Compute
            double DistanceKm(double lat1, double lon1, double lat2, double lon2)
            {
                double R = 6371.0;
                double dLat = (lat2 - lat1) * Math.PI / 180.0;
                double dLon = (lon2 - lon1) * Math.PI / 180.0;
                double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                           Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) *
                           Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
                double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                return R * c;
            }

            var ranked = new List<(RecommendedCoachDto dto, double score)>();

            foreach (var c in coaches)
            {
                // nearest gym distance
                var nearest = c.CoachGyms
                    .Select(cg => new
                    {
                        cg.GymId,
                        cg.Gym.Name,
                        Dist = DistanceKm(req.Latitude, req.Longitude, cg.Gym.Latitude, cg.Gym.Longitude)
                    })
                    .OrderBy(x => x.Dist)
                    .FirstOrDefault();

                if (nearest == null) continue;
                if (nearest.Dist > req.MaxDistanceKm) continue;

                var plans = c.Plans.AsEnumerable();
                if (req.Goal.HasValue)
                    plans = plans.Where(p => p.Goal == req.Goal.Value);

                if (req.BudgetMin.HasValue) plans = plans.Where(p => p.Price >= req.BudgetMin.Value);
                if (req.BudgetMax.HasValue) plans = plans.Where(p => p.Price <= req.BudgetMax.Value);

                if (!plans.Any()) continue;

                var minPrice = plans.Min(p => p.Price);
                var matchTitles = plans.Select(p => p.Title).Take(3).ToArray();

                var avgRating = c.Reviews.Any() ? c.Reviews.Average(r => r.Rating) : 0.0;
                var countReviews = c.Reviews.Count;

                // Score (tweak weights as desired)
                double goalScore = req.Goal.HasValue ? 1.0 : 0.7; // if goal constrained, strong match
                double priceScore = 1.0; // already filtered into budget; could refine with closeness to BudgetMin/Max
                double ratingScore = avgRating / 5.0; // 0..1
                double distanceScore = 1.0 - (nearest.Dist / Math.Max(1.0, req.MaxDistanceKm)); // closer is better

                double score = 0.3 * goalScore + 0.25 * priceScore + 0.3 * ratingScore + 0.15 * distanceScore;

                ranked.Add((new RecommendedCoachDto
                {
                    CoachId = c.Id,
                    CoachName = c.Name,
                    Specialization = c.Specialization,
                    DistanceKm = Math.Round(nearest.Dist, 2),
                    GymId = nearest.GymId,
                    GymName = nearest.Name,
                    MinPlanPrice = minPrice,
                    AvgRating = Math.Round(avgRating, 2),
                    ReviewsCount = countReviews,
                    MatchingPlanTitles = matchTitles
                }, score));
            }

            var result = ranked
                .OrderByDescending(x => x.score)
                .Take(req.Take)
                .Select(x => x.dto)
                .ToList();

            return Ok(result);
        }
    }
}
