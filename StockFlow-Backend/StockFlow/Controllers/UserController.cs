using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using FocusSpace.DatabaseContext;
using FocusSpace.Models;
using FocusSpace.Requests;
using FocusSpace.Encrypt;
using FocusSpace.Services;

namespace FocusSpace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IJwtService _jwtService;

        public UserController(DataContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || !PasswordEncryptor.Verify(request.Password, user.Password))
                return Unauthorized("Credenciais inválidas");

            var token = _jwtService.GenerateToken(user.Id, user.Username, user.Role);
            return Ok(new LoginResponse { Id = user.Id, Username = user.Username, Role = user.Role, Token = token });
        }
    }

    public class LoginResponse
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Role { get; set; }
        public required string Token { get; set; }
    }
}
