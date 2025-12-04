using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Models;

namespace Api.Controllers {
  [ApiController]
  [Route("api/[controller]")]
  public class ProductsController:ControllerBase {
    private readonly AppDb _db;
    public ProductsController(AppDb db){_db=db;}

    [HttpGet]
    public async Task<IActionResult> Get() =>
      Ok(await _db.Products.Include(p=>p.Category).ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetId(int id){
      var p = await _db.Products.Include(p=>p.Category).FirstOrDefaultAsync(p=>p.Id==id);
      return p==null? NotFound(): Ok(p);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Product p){
      if(!ModelState.IsValid) return BadRequest(ModelState);
      _db.Products.Add(p);
      await _db.SaveChangesAsync();
      return CreatedAtAction(nameof(GetId), new{id=p.Id}, p);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Product p){
      if(id!=p.Id) return BadRequest();
      if(!ModelState.IsValid) return BadRequest(ModelState);
      if(!await _db.Products.AnyAsync(x=>x.Id==id)) return NotFound();
      _db.Update(p);
      await _db.SaveChangesAsync();
      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id){
      var p = await _db.Products.FindAsync(id);
      if(p==null) return NotFound();
      _db.Remove(p);
      await _db.SaveChangesAsync();
      return NoContent();
    }
  }
}