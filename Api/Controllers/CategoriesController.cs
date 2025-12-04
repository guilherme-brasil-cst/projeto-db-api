using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Models;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDb _db;

        public CategoriesController(AppDb db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get() =>
            Ok(await _db.Categories.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetId(int id)
        {
            var c = await _db.Categories.FindAsync(id);
            return c == null ? NotFound() : Ok(c);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Category c)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _db.Categories.Add(c);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetId), new { id = c.Id }, c);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Category c)
        {
            if (id != c.Id)
                return BadRequest();

            if (!await _db.Categories.AnyAsync(x => x.Id == id))
                return NotFound();

            _db.Update(c);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var c = await _db.Categories.FindAsync(id);
            if (c == null)
                return NotFound();

            _db.Remove(c);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
