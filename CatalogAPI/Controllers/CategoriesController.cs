using CatalogAPI.Context;
using CatalogAPI.Models;
using Microsoft.AspNetCore.Http;
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
        public ActionResult<IEnumerable<Category>> Get()
        {
            try
            {
                var categories = _context.Categories.AsNoTracking().ToList();

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
        public ActionResult<Category> Get(int id)
        {
            try
            {
                var category = _context
                    .Categories.AsNoTracking()
                    .FirstOrDefault(x => x.CategoryId == id);

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
        public ActionResult<IEnumerable<Category>> GetCategoriesProducts()
        {
            try
            {
                var categories = _context
                    .Categories.AsNoTracking()
                    .Include(x => x.Products)
                    .ToList();

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
        public ActionResult<Category> Post(Category category)
        {
            try
            {
                if (category == null)
                    return BadRequest("The category is invalid...");

                _context.Categories.Add(category);
                _context.SaveChanges();
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
        public ActionResult Put(int id, Category category)
        {
            try
            {
                if (id != category.CategoryId)
                    return BadRequest(
                        $"Id={id} does not match CategoryId={category.CategoryId}..."
                    );

                _context.Categories.Update(category);
                _context.SaveChanges();
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
        public ActionResult<Category> Delete(int id)
        {
            try
            {
                var category = _context.Categories.FirstOrDefault(x => x.CategoryId == id);

                if (category == null)
                    return NotFound($"Category with id={id} not found...");

                _context.Categories.Remove(category);
                _context.SaveChanges();
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
