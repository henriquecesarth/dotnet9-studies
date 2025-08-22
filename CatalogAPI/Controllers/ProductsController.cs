using CatalogAPI.Context;
using CatalogAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAsync()
        {
            var products = await _context.Products.AsNoTracking().ToListAsync();

            if (products == null)
                return NotFound("Any product was found...");

            return products;
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> GetAsync(int id)
        {
            var product = await _context
                .Products.AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductId == id);

            if (product == null)
                return NotFound($"Product with id={id} not found...");

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostAsync(Product product)
        {
            if (product == null)
                return BadRequest("The product is invalid...");

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return new CreatedAtRouteResult("GetProduct", new { id = product.ProductId }, product);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Product>> PutAsync(int id, Product product)
        {
            if (id != product.ProductId)
                return BadRequest($"Id={id} does not match ProductId={product.ProductId}...");

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Product>> DeleteAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);

            if (product == null)
                return NotFound($"Product with id={id} not found...");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }
    }
}
