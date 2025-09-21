using FitnessBakcend.Models;
using FitnessBakcend.Data;
using FitnessBakcend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessBakcend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly FitnessContext _db;
        public ClientsController(FitnessContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetAll()
            => await _db.Clients.AsNoTracking().ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Client>> GetById(int id)
        {
            var client = await _db.Clients.FindAsync(id);
            return client is null ? NotFound() : client;
        }

        [HttpPost]
        public async Task<ActionResult<Client>> Create(Client client)
        {
            _db.Clients.Add(client);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Client client)
        {
            if (id != client.Id) return BadRequest("Mismatched id");

            var exists = await _db.Clients.AnyAsync(x => x.Id == id);
            if (!exists) return NotFound();

            _db.Entry(client).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.Clients.FindAsync(id);
            if (entity is null) return NotFound();

            _db.Clients.Remove(entity);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
