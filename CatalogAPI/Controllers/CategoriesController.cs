using CatalogAPI.DTOs;
using CatalogAPI.DTOs.Mappings;
using CatalogAPI.Filters;
using CatalogAPI.Models;
using CatalogAPI.Repositories;
using MapsterMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CategoriesController(
            ILogger<CategoriesController> logger,
            IUnitOfWork uof,
            IMapper mapper
        )
        {
            _logger = logger;
            _uof = uof;
            _mapper = mapper;
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

            var categoriesDTO = _mapper.Map<List<CategoryDTO>>(categories);

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

            var categoryDTO = _mapper.Map<CategoryDTO>(category);

            return categoryDTO;
        }

        [HttpGet("Products")]
        public ActionResult<IEnumerable<CategoryDTO>> GetCategoriesProducts()
        {
            _logger.LogInformation("######### GET Categories/Products #########");

            var categories = _uof.CategoryRepository.GetCategoriesProducts();

            if (categories == null)
                return NotFound("Any category was found...");

            var categoriesDTO = _mapper.Map<List<CategoryProductsDTO>>(categories);

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

            var category = _mapper.Map<Category>(categoryDTO);

            var createdCategory = _uof.CategoryRepository.Create(category);
            _uof.Commit();

            var createdCategoryDTO = _mapper.Map<CategoryDTO>(createdCategory);

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

            var category = _mapper.Map<Category>(categoryDTO);

            var updatedCategory = _uof.CategoryRepository.Update(category);
            _uof.Commit();

            var updatedCategoryDTO = _mapper.Map<CategoryDTO>(updatedCategory);

            return updatedCategoryDTO;
        }

        [HttpPatch("{id:int}")]
        public ActionResult<CategoryDTO> Patch(int id, JsonPatchDocument<CategoryDTO> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("The category is invalid...");

            var category = _uof.CategoryRepository.Get(c => c.CategoryId == id);

            if (category == null)
                return NotFound($"Category with id={id} not found...");

            var categoryToPatch = _mapper.Map<CategoryDTO>(category);

            patchDoc.ApplyTo(categoryToPatch);

            return categoryToPatch;
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoryDTO> Delete(int id)
        {
            var deletedCategory = _uof.CategoryRepository.Delete(id);
            _uof.Commit();

            var deletedCategoryDTO = _mapper.Map<CategoryDTO>(deletedCategory);

            return deletedCategoryDTO;
        }
    }
}
