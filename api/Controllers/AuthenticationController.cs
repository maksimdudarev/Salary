using MD.Salary.WebApi.Core.Interfaces;
using MD.Salary.WebApi.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Sodium;
using System.Threading.Tasks;

namespace MD.Salary.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public AuthenticationController(IUserRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Authentication/a@b.com
        [HttpGet("{name}")]
        public async Task<IActionResult> GetUser(string name)
        {
            var item = await _repository.GetByNameAsync(name);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST: api/Authentication
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existed = await _repository.GetByNameAsync(item.Name);
            if (existed != null)
            {
                return Conflict();
            }
            item.Password = PasswordHash.ScryptHashString(item.Password, PasswordHash.Strength.Medium);
            await _repository.AddAsync(item);
            return CreatedAtAction(nameof(GetUser), new { name = item.Name }, item);
        }
    }
}
