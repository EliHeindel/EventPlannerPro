using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventPlannerPro.Shared;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class EventsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EventsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetEvents()
    {
        var events = await _context.Events.ToListAsync();
        return Ok(events);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] Event event)
    {
        _context.Events.Add(event);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEvent), new { id = event.Id }, event);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEvent(int id)
    {
        var event = await _context.Events.FindAsync(id);
        if (event == null)
        {
            return NotFound();
        }
        return Ok(event);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(int id, [FromBody] Event event)
    {
        if (id != event.Id)
        {
            return BadRequest();
        }

        _context.Entry(event).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var event = await _context.Events.FindAsync(id);
        if (event == null)
        {
            return NotFound();
        }

        _context.Events.Remove(event);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
