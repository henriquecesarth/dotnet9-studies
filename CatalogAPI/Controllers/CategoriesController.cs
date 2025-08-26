using CatalogAPI.Filters;
using CatalogAPI.Models;
using CatalogAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger _logger;

        public CategoriesController(
            ILogger<CategoriesController> logger,
            ICategoryRepository categoryRepository
        )
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Category>> Get()
        {
            _logger.LogInformation($"######### GET Categories/ #########");

            var categories = _categoryRepository.GetAll();

            if (categories == null)
            {
                _logger.LogWarning("Any category was found...");
                return NotFound("Any category was found...");
            }

            return Ok(categories);
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public ActionResult<Category> Get(int id)
        {
            _logger.LogInformation($"######### GET Categories/{id} #########");

            var category = _categoryRepository.Get(c => c.CategoryId == id);

            if (category == null)
            {
                _logger.LogWarning($"Category with id={id} not found...");
                return NotFound($"Category with id={id} not found...");
            }

            return category;
        }

        [HttpGet("Products")]
        public ActionResult<IEnumerable<Category>> GetCategoriesProducts()
        {
            _logger.LogInformation("######### GET Categories/Products #########");

            var categories = _categoryRepository.GetCategoriesProducts();

            if (categories == null)
                return NotFound("Any category was found...");

            return Ok(categories);
        }

        [HttpPost]
        public ActionResult Post(Category category)
        {
            _logger.LogInformation("######### POST Categories #########");

            if (category == null)
            {
                _logger.LogWarning("The category is invalid...");
                return BadRequest("The category is invalid...");
            }

            var createdCategory = _categoryRepository.Create(category);

            return new CreatedAtRouteResult(
                "GetCategory",
                new { id = createdCategory.CategoryId },
                category
            );
        }

        [HttpPut("{id:int}")]
        public ActionResult<Category> Put(int id, Category category)
        {
            _logger.LogInformation("######### PUT Categories #########");

            if (category.CategoryId != id)
            {
                _logger.LogWarning("The categories id does not match...");
                return BadRequest("The categories id does not match...");
            }

            var updatedCategory = _categoryRepository.Update(category);
            return updatedCategory;
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Category> Delete(int id)
        {
            var deletedCategory = _categoryRepository.Delete(id);

            return deletedCategory;
        }
    }
}
