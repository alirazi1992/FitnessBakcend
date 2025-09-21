using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessBakcend.Data;
using FitnessBakcend.Models;

namespace FitnessBakcend.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly FitnessContext _db;
        public PortfolioController(FitnessContext db) => _db = db;

        [HttpGet("coach/{coachId:int}")]
        public async Task<ActionResult<IEnumerable<PortfolioPost>>> Get(int coachId) =>
            await _db.PortfolioPosts.Where(p => p.CoachId == coachId)
                .OrderByDescending(p => p.CreatedAt)
                .AsNoTracking().ToListAsync();

        [HttpPost]
        public async Task<ActionResult<PortfolioPost>> Create(PortfolioPost post)
        {
            _db.PortfolioPosts.Add(post);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { coachId = post.CoachId }, post);
        }
    }
}
