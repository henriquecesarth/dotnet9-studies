using CatalogAPI.DTOs;
using CatalogAPI.DTOs.Mappings;
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
        private readonly IUnitOfWork _uof;
        private readonly ILogger _logger;

        public CategoriesController(ILogger<CategoriesController> logger, IUnitOfWork uof)
        {
            _logger = logger;
            _uof = uof;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<CategoryDTO>> Get()
        {
            _logger.LogInformation($"######### GET Categories/ #########");

            var categories = _uof.CategoryRepository.GetAll();

            if (categories == null)
            {
                _logger.LogWarning("Any category was found...");
                return NotFound("Any category was found...");
            }

            var categoriesDTO = categories.MapToCategoryDTOList();

            return Ok(categoriesDTO);
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public ActionResult<CategoryDTO> Get(int id)
        {
            _logger.LogInformation($"######### GET Categories/{id} #########");

            var category = _uof.CategoryRepository.Get(c => c.CategoryId == id);

            if (category == null)
            {
                _logger.LogWarning($"Category with id={id} not found...");
                return NotFound($"Category with id={id} not found...");
            }

            var categoryDTO = category.MapToCategoryDTO();

            return categoryDTO;
        }

        [HttpGet("Products")]
        public ActionResult<IEnumerable<CategoryDTO>> GetCategoriesProducts()
        {
            _logger.LogInformation("######### GET Categories/Products #########");

            var categories = _uof.CategoryRepository.GetCategoriesProducts();

            if (categories == null)
                return NotFound("Any category was found...");

            var categoriesDTO = categories.MapToCategoryDTOList();

            return Ok(categoriesDTO);
        }

        [HttpPost]
        public ActionResult<CategoryDTO> Post(CategoryDTO categoryDTO)
        {
            _logger.LogInformation("######### POST Categories #########");

            if (categoryDTO == null)
            {
                _logger.LogWarning("The category is invalid...");
                return BadRequest("The category is invalid...");
            }

            var category = categoryDTO.MapToCategory();

            var createdCategory = _uof.CategoryRepository.Create(category);
            _uof.Commit();

            var createdCategoryDTO = createdCategory.MapToCategoryDTO();

            return new CreatedAtRouteResult(
                "GetCategory",
                new { id = createdCategoryDTO.CategoryId },
                createdCategoryDTO
            );
        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoryDTO> Put(int id, CategoryDTO categoryDTO)
        {
            _logger.LogInformation("######### PUT Categories #########");

            if (categoryDTO.CategoryId != id)
            {
                _logger.LogWarning("The categories id does not match...");
                return BadRequest("The categories id does not match...");
            }

            var category = categoryDTO.MapToCategory();

            var updatedCategory = _uof.CategoryRepository.Update(category);
            _uof.Commit();

            var updatedCategoryDTO = updatedCategory.MapToCategoryDTO();

            return updatedCategoryDTO;
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoryDTO> Delete(int id)
        {
            var deletedCategory = _uof.CategoryRepository.Delete(id);
            _uof.Commit();

            var deletedCategoryDTO = deletedCategory.MapToCategoryDTO();

            return deletedCategoryDTO;
        }
    }
}
