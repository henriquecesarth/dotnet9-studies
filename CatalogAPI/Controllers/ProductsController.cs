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
            try
            {
                var products = await _context.Products.AsNoTracking().ToListAsync();

                if (products == null)
                    return NotFound("Any product was found...");

                return products;
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error ocurred while processing your request."
                );
            }
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> GetAsync(int id)
        {
            try
            {
                var product = await _context
                    .Products.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProductId == id);

                if (product == null)
                    return NotFound($"Product with id={id} not found...");

                return product;
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error ocurred while processing your request."
                );
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostAsync(Product product)
        {
            try
            {
                if (product == null)
                    return BadRequest("The product is invalid...");

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return new CreatedAtRouteResult(
                    "GetProduct",
                    new { id = product.ProductId },
                    product
                );
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error ocurred while processing your request."
                );
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Product>> PutAsync(int id, Product product)
        {
            try
            {
                if (id != product.ProductId)
                    return BadRequest($"Id={id} does not match ProductId={product.ProductId}...");

                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                return Ok(product);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error ocurred while processing your request."
                );
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Product>> DeleteAsync(int id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);

                if (product == null)
                    return NotFound($"Product with id={id} not found...");

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return Ok(product);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error ocurred while processing your request."
                );
            }
        }
    }
}
