using MD.Salary.WebApi.Core.Interfaces;
using MD.Salary.WebApi.Core.Models;
using MD.Salary.WebApi.Middleware;
using Microsoft.AspNetCore.Mvc;
using Sodium;
using System;
using System.Threading.Tasks;

namespace MD.Salary.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ICombineRepository _repository;

        public AuthenticationController(ICombineRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Authentication/a@b.com
        [HttpGet("{name}")]
        public async Task<IActionResult> GetUser(string name)
        {
            var item = await _repository.GetUserByNameAsync(name);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST: api/Authentication/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existed = await _repository.GetUserByNameAsync(item.Name);
            if (existed != null)
            {
                return Conflict();
            }
            item.Password = PasswordHash.ScryptHashString(item.Password, PasswordHash.Strength.Medium);
            await _repository.AddUserAsync(item);
            return CreatedAtAction(nameof(GetUser), new { name = item.Name }, item);
        }

        // GET: api/Authentication/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userlist = await _repository.UserListAsync();
            var user = userlist.Find(i => PasswordHash.ScryptHashStringVerify(i.Password, item.Password));
            if (user == null)
            {
                return NotFound();
            }
            var token = new Token { 
                User = user.ID, 
                Value = Convert.ToBase64String(SecretBox.GenerateKey())
            };
            await _repository.AddTokenAsync(token);
            return Ok(token.Value);
        }

        // DELETE: api/Authentication/logout
        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Items.TryGetValue(AuthenticationMiddleware.AuthenticationMiddlewareKey, out var middlewareValue);
            var item = (Token)middlewareValue;
            await _repository.DeleteTokenAsync(item);
            return Ok();
        }
    }
}
