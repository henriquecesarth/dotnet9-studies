using CatalogAPI.Context;
using CatalogAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAsync()
        {
            try
            {
                var categories = await _context.Categories.AsNoTracking().ToListAsync();

                if (categories == null)
                    return NotFound("Any category was found...");

                return categories;
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error ocurred while processing your request."
                );
            }
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public async Task<ActionResult<Category>> GetAsync(int id)
        {
            try
            {
                var category = await _context
                    .Categories.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.CategoryId == id);

                if (category == null)
                    return NotFound($"Category with id={id} not found...");

                return category;
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error ocurred while processing your request."
                );
            }
        }

        [HttpGet("Products")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesProductsAsync()
        {
            try
            {
                var categories = await _context
                    .Categories.AsNoTracking()
                    .Include(x => x.Products)
                    .ToListAsync();

                if (categories == null)
                    return NotFound("Any category was found...");

                return categories;
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
        public async Task<ActionResult> PostAsync(Category category)
        {
            try
            {
                if (category == null)
                    return BadRequest("The category is invalid...");

                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return new CreatedAtRouteResult(
                    "GetCategory",
                    new { id = category.CategoryId },
                    category
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
        public async Task<ActionResult> PutAsync(int id, Category category)
        {
            try
            {
                if (id != category.CategoryId)
                    return BadRequest(
                        $"Id={id} does not match CategoryId={category.CategoryId}..."
                    );

                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                return Ok(category);
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
        public async Task<ActionResult<Category>> DeleteAsync(int id)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(x =>
                    x.CategoryId == id
                );

                if (category == null)
                    return NotFound($"Category with id={id} not found...");

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return Ok(category);
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
