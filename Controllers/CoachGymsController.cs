using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessBakcend.Data;
using FitnessBakcend.Models;

namespace FitnessBakcend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoachGymsController : ControllerBase
    {
        private readonly FitnessContext _db;
        public CoachGymsController(FitnessContext db) => _db = db;

        [HttpPost]
        public async Task<IActionResult> Assign([FromBody] CoachGym dto)
        {
            var exists = await _db.CoachGyms.AnyAsync(x => x.CoachId == dto.CoachId && x.GymId == dto.GymId);
            if (exists) return Conflict("Coach already assigned to this gym.");

            _db.CoachGyms.Add(dto);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetByCoach), new { coachId = dto.CoachId }, dto);
        }

        [HttpDelete]
        public async Task<IActionResult> Unassign([FromQuery] int coachId, [FromQuery] int gymId)
        {
            var cg = await _db.CoachGyms.FindAsync(coachId, gymId);
            if (cg is null) return NotFound();
            _db.CoachGyms.Remove(cg);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("coach/{coachId:int}")]
        public async Task<ActionResult<IEnumerable<CoachGym>>> GetByCoach(int coachId) =>
            await _db.CoachGyms.Include(x => x.Gym).Where(x => x.CoachId == coachId).AsNoTracking().ToListAsync();

        [HttpGet("gym/{gymId:int}")]
        public async Task<ActionResult<IEnumerable<CoachGym>>> GetByGym(int gymId) =>
            await _db.CoachGyms.Include(x => x.Coach).Where(x => x.GymId == gymId).AsNoTracking().ToListAsync();
    }
}
