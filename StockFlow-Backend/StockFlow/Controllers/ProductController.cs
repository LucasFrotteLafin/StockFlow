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
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAll()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpGet("sku/{sku}")]
        public async Task<ActionResult<Product>> GetBySKU(string sku)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.SKU == sku);
            
            if (product == null)
                return NotFound("Produto não encontrado com este SKU");
            
            return Ok(product);
        }

        [HttpGet("low-stock")]
        public async Task<ActionResult<List<Product>>> GetLowStockProducts()
        {
            var products = await _context.Products
                .Where(p => p.QuantityInStock < p.MinimumStock)
                .ToListAsync();
            
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create(CreateProductRequest request)
        {
            try
            {
                // Verificar se já existe produto com mesmo SKU
                var existingSKU = await _context.Products
                    .FirstOrDefaultAsync(p => p.SKU == request.SKU);
                
                if (existingSKU != null)
                    return BadRequest("Já existe um produto cadastrado com este SKU");

                var product = new Product
                {
                    Name = request.Name,
                    SKU = request.SKU,
                    Category = request.Category,
                    Price = request.Price,
                    MinimumStock = request.MinimumStock,
                    QuantityInStock = 0
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao criar produto: {ex.Message}");
                Console.WriteLine($"❌ Stack trace: {ex.StackTrace}");
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProductRequest request)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            // Verificar se já existe outro produto com mesmo SKU
            var existingSKU = await _context.Products
                .FirstOrDefaultAsync(p => p.SKU == request.SKU && p.Id != id);
            
            if (existingSKU != null)
                return BadRequest("Já existe outro produto cadastrado com este SKU");

            product.Name = request.Name;
            product.SKU = request.SKU;
            product.Category = request.Category;
            product.Price = request.Price;
            product.QuantityInStock = request.QuantityInStock;
            product.MinimumStock = request.MinimumStock;

            await _context.SaveChangesAsync();
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
