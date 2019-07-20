using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MD.Salary.WebApi.Models;

namespace MD.Salary.WebApi.Controllers
{
    [Route("api/blogging")]
    [ApiController]
    public class BloggingController : ControllerBase
    {
        private readonly BloggingContext _context;

        public BloggingController(BloggingContext context)
        {
            _context = context;

            if (_context.Blogs.Count() == 0)
            {
                // Create a new Blog if collection is empty,
                // which means you can't delete all Blogs.
                _context.Blogs.Add(new Blog { Url = "google.com" });
                _context.SaveChanges();
            }
        }
        // GET: api/Blogging
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Blog>>> GetBloggingItems()
        {
            return await _context.Blogs.ToListAsync();
        }

        // GET: api/Blogging/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Blog>> GetBloggingItem(long id)
        {
            var bloggingItem = await _context.Blogs.FindAsync(id);

            if (bloggingItem == null)
            {
                return NotFound();
            }

            return bloggingItem;
        }
        // POST: api/Blogging
        [HttpPost]
        public async Task<ActionResult<Blog>> PostBloggingItem(Blog item)
        {
            _context.Blogs.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBloggingItem), new { id = item.BlogId }, item);
        }
        // PUT: api/Blogging/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBloggingItem(long id, Blog item)
        {
            if (id != item.BlogId)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // DELETE: api/Blogging/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBloggingItem(long id)
        {
            var bloggingItem = await _context.Blogs.FindAsync(id);

            if (bloggingItem == null)
            {
                return NotFound();
            }

            _context.Blogs.Remove(bloggingItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}