using MD.Salary.WebApi.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace MD.Salary.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public void Authentication(User user)
        {
            var password = user.Password;
            var pbkdf2Hash = CryptographyService.HashPasswordUsingPBKDF2(password);

        }
    }
}
