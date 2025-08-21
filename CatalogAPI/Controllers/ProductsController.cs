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
        public ActionResult<IEnumerable<Product>> Get()
        {
            try
            {
                var products = _context.Products.AsNoTracking().ToList();

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
        public ActionResult<Product> Get(int id)
        {
            try
            {
                var product = _context
                    .Products.AsNoTracking()
                    .FirstOrDefault(x => x.ProductId == id);

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
        public ActionResult<Product> Post(Product product)
        {
            try
            {
                if (product == null)
                    return BadRequest("The product is invalid...");

                _context.Products.Add(product);
                _context.SaveChanges();
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
        public ActionResult<Product> Put(int id, Product product)
        {
            try
            {
                if (id != product.ProductId)
                    return BadRequest($"Id={id} does not match ProductId={product.ProductId}...");

                _context.Products.Update(product);
                _context.SaveChanges();

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
        public ActionResult<Product> Delete(int id)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(x => x.ProductId == id);

                if (product == null)
                    return NotFound($"Product with id={id} not found...");

                _context.Products.Remove(product);
                _context.SaveChanges();

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
