using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using FocusSpace.DatabaseContext;
using FocusSpace.Models;
using FocusSpace.Encrypt;

namespace FocusSpace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    public class UserRequestController : ControllerBase
    {
        private readonly DataContext _context;

        public UserRequestController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var userExists = await _context.Users
                .AnyAsync(u => u.Username == request.Username);

            if (userExists)
                return BadRequest("Username ja esta em uso.");

            var requestExists = await _context.UserRequests
                .AnyAsync(r => r.Username == request.Username && r.Status != "Rejeitado");

            if (requestExists)
                return BadRequest("Ja existe uma solicitacao pendente para este username.");

            var userRequest = new UserRequest
            {
                Username = request.Username,
                PasswordHash = PasswordEncryptor.Encrypt(request.Password),
                Status = "Pendente",
                RequestDate = DateTime.UtcNow
            };

            _context.UserRequests.Add(userRequest);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Solicitacao enviada! Aguarde aprovacao do administrador." });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPending()
        {
            var requests = await _context.UserRequests
                .Where(r => r.Status == "Pendente")
                .Select(r => new { r.Id, r.Username, r.Status, r.RequestDate })
                .ToListAsync();

            return Ok(requests);
        }

        [HttpGet("approved")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetApproved()
        {
            var users = await _context.Users
                .Where(u => u.Role != "Admin")
                .Select(u => new { u.Id, u.Username, u.Role })
                .ToListAsync();

            return Ok(users);
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            var request = await _context.UserRequests.FindAsync(id);

            if (request == null)
                return NotFound("Solicitacao nao encontrada.");

            if (request.Status != "Pendente")
                return BadRequest("Esta solicitacao ja foi processada.");

            _context.Users.Add(new User
            {
                Username = request.Username,
                Password = request.PasswordHash,
                Role = "User"
            });

            request.Status = "Aprovado";
            request.ApprovedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = $"Usuario {request.Username} aprovado com sucesso." });
        }

        [HttpPost("{id}/reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject(int id)
        {
            var request = await _context.UserRequests.FindAsync(id);

            if (request == null)
                return NotFound("Solicitacao nao encontrada.");

            if (request.Status != "Pendente")
                return BadRequest("Esta solicitacao ja foi processada.");

            request.Status = "Rejeitado";

            await _context.SaveChangesAsync();

            return Ok(new { message = $"Solicitacao de {request.Username} rejeitada." });
        }
    }

    public class RegisterRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
