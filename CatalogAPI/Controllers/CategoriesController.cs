using CatalogAPI.Context;
using CatalogAPI.Filters;
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
        private readonly ILogger _logger;

        public CategoriesController(AppDbContext context, ILogger<CategoriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Category>>> GetAsync()
        {
            _logger.LogInformation($"######### GET Categories/ #########");

            var categories = await _context.Categories.AsNoTracking().ToListAsync();

            if (categories == null)
                return NotFound("Any category was found...");

            return categories;
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public async Task<ActionResult<Category>> GetAsync(int id)
        {
            _logger.LogInformation($"######### GET Categories/{id} #########");

            var category = await _context
                .Categories.AsNoTracking()
                .FirstOrDefaultAsync(x => x.CategoryId == id);

            if (category == null)
                return NotFound($"Category with id={id} not found...");

            return category;
        }

        [HttpGet("Products")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesProductsAsync()
        {
            _logger.LogInformation("######### GET Categories/Products #########");

            var categories = await _context
                .Categories.AsNoTracking()
                .Include(x => x.Products)
                .ToListAsync();

            if (categories == null)
                return NotFound("Any category was found...");

            return categories;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Category category)
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

        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutAsync(int id, Category category)
        {
            if (id != category.CategoryId)
                return BadRequest($"Id={id} does not match CategoryId={category.CategoryId}...");

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return Ok(category);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Category>> DeleteAsync(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);

            if (category == null)
                return NotFound($"Category with id={id} not found...");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }
    }
}
