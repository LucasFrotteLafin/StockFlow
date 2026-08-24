using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using FocusSpace.DatabaseContext;
using FocusSpace.Models;
using FocusSpace.Requests;
using FocusSpace.Encrypt;

namespace FocusSpace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(LoginRequest request)
        {
            var userExists = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (userExists != null)
                return BadRequest("Usuário já existe");

            var user = new User
            {
                Username = request.Username,
                Password = PasswordEncryptor.Encrypt(request.Password),
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || !PasswordEncryptor.Verify(request.Password, user.Password))
                return Unauthorized("Credenciais inválidas");

            return Ok(new LoginResponse { Id = user.Id, Username = user.Username, Role = user.Role });
        }
    }

    public class LoginResponse
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Role { get; set; }
    }
}
