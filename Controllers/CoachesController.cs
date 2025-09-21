using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessBakcend.Data;
using FitnessBakcend.Models;

namespace FitnessBakcend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoachesController : ControllerBase
    {
        private readonly FitnessContext _db;
        public CoachesController(FitnessContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Coach>>> Get() =>
            await _db.Coaches.AsNoTracking().ToListAsync();

        [HttpPost]
        public async Task<ActionResult<Coach>> Create(Coach coach)
        {
            _db.Coaches.Add(coach);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = coach.Id }, coach);
        }
    }
}
