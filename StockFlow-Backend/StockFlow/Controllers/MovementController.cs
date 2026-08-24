using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using FocusSpace.DatabaseContext;
using FocusSpace.Models;
using FocusSpace.Requests;

namespace FocusSpace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    public class MovementController : ControllerBase
    {
        private readonly DataContext _context;

        public MovementController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Movement>>> GetAll()
        {
            var movements = await _context.Movements.ToListAsync();
            return Ok(movements);
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<List<Movement>>> GetByProduct(int productId)
        {
            var movements = await _context.Movements
                .Where(m => m.ProductId == productId)
                .ToListAsync();
            return Ok(movements);
        }

        [HttpPost]
        public async Task<ActionResult<Movement>> Create(CreateMovementRequest request)
        {
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
                return NotFound("Produto não encontrado");

            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
                return NotFound("Usuário não encontrado");

            if (request.Type == "Saída" && product.QuantityInStock < request.Quantity)
                return BadRequest("Quantidade insuficiente em estoque");

            var movement = new Movement
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                Type = request.Type,
                Reason = request.Reason,
                MovementDate = DateTime.UtcNow,
                UserId = request.UserId,
                UserName = user.Username
            };

            if (request.Type == "Entrada")
                product.QuantityInStock += request.Quantity;
            else
                product.QuantityInStock -= request.Quantity;

            _context.Movements.Add(movement);
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), movement);
        }
    }
}
