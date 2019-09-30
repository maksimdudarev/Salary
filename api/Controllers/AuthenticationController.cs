using MD.Salary.WebApi.Core.Interfaces;
using MD.Salary.WebApi.Core.Models;
using Microsoft.AspNetCore.Mvc;
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

        // GET: api/Authentication/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(long id)
        {
            var item = await _repository.GetByIdAsync(id);
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

            var password = item.Password;
            var pbkdf2Hash = CryptographyService.HashPasswordUsingPBKDF2(password);
            item.Password = pbkdf2Hash;

            await _repository.AddAsync(item);
            return CreatedAtAction(nameof(GetUser), new { id = item.ID }, item);
        }
    }
}
