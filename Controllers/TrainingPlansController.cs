using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessBakcend.Data;
using FitnessBakcend.Models;

namespace FitnessBakcend.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class TrainingPlansController : ControllerBase
    {
        private readonly FitnessContext _db;
        public TrainingPlansController(FitnessContext db) => _db = db;

        [HttpGet("coach/{coachId:int}")]
        public async Task<ActionResult<IEnumerable<TrainingPlan>>> GetByCoach(int coachId) =>
            await _db.TrainingPlans.Where(p => p.CoachId == coachId)
                .AsNoTracking().ToListAsync();

        [HttpPost]
        public async Task<ActionResult<TrainingPlan>> Create(TrainingPlan plan)
        {
            _db.TrainingPlans.Add(plan);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetByCoach), new { coachId = plan.CoachId }, plan);
        }
    }
}
