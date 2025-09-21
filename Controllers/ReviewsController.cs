using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessBakcend.Data;
using FitnessBakcend.Models;

namespace FitnessBakcend.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly FitnessContext _db;
        public ReviewsController(FitnessContext db) => _db = db;

        [HttpGet("coach/{coachId:int}")]
        public async Task<ActionResult<object>> GetForCoach(int coachId)
        {
            var reviews = await _db.Reviews.Where(r => r.CoachId == coachId)
                .OrderByDescending(r => r.CreatedAt)
                .AsNoTracking().ToListAsync();

            var avg = reviews.Any() ? reviews.Average(r => r.Rating) : 0.0;
            return Ok(new { average = avg, count = reviews.Count, items = reviews });
        }

        [HttpPost]
        public async Task<ActionResult<Review>> Create(Review review)
        {
            _db.Reviews.Add(review);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetForCoach), new { coachId = review.CoachId }, review);
        }
    }
}
