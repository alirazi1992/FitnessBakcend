using FitnessBakcend.Models;
using FitnessBakcend.Data;
using FitnessBakcend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessBakcend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GymsController : ControllerBase
    {
        private readonly FitnessContext _db;
        public GymsController(FitnessContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gym>>> GetAll()
            => await _db.Gyms.AsNoTracking().ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Gym>> GetById(int id)
        {
            var gym = await _db.Gyms.FindAsync(id);
            return gym is null ? NotFound() : gym;
        }

        [HttpPost]
        public async Task<ActionResult<Gym>> Create(Gym gym)
        {
            _db.Gyms.Add(gym);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = gym.Id }, gym);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Gym gym)
        {
            if (id != gym.Id) return BadRequest("Mismatched id");

            var exists = await _db.Gyms.AnyAsync(x => x.Id == id);
            if (!exists) return NotFound();

            _db.Entry(gym).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.Gyms.FindAsync(id);
            if (entity is null) return NotFound();

            _db.Gyms.Remove(entity);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
