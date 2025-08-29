using CatalogAPI.DTOs;
using CatalogAPI.DTOs.Mappings;
using CatalogAPI.Models;
using CatalogAPI.Repositories;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDTO>> Get()
        {
            var products = _uof.ProductRepository.GetAll();

            if (products == null)
                return NotFound("Any product was found...");

            var productsDTO = _mapper.Map<List<ProductDTO>>(products);

            return Ok(productsDTO);
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public ActionResult<ProductDTO> Get(int id)
        {
            var product = _uof.ProductRepository.Get(p => p.ProductId == id);

            if (product == null)
                return NotFound($"Product with id={id} not found...");

            var productDTO = _mapper.Map<ProductDTO>(product);

            return productDTO;
        }

        [HttpGet("Categories/{categoryId:int}")]
        public ActionResult<IEnumerable<ProductDTO>> GetProductsByCategory(int categoryId)
        {
            var products = _uof.ProductRepository.GetProductsByCategory(categoryId);

            if (products == null || !products.Any())
                return NotFound($"Any product was found for category with id={categoryId}...");

            var productsDTO = _mapper.Map<List<ProductDTO>>(products);

            return Ok(productsDTO);
        }

        [HttpPost]
        public ActionResult<ProductDTO> Post(ProductDTO productDTO)
        {
            var product = _mapper.Map<Product>(productDTO);

            var createdProduct = _uof.ProductRepository.Create(product);
            _uof.Commit();

            var createdProductDTO = _mapper.Map<ProductDTO>(createdProduct);

            return new CreatedAtRouteResult(
                "GetProduct",
                new { id = createdProductDTO.ProductId },
                createdProductDTO
            );
        }

        [HttpPut("{id:int}")]
        public ActionResult<ProductDTO> Put(int id, ProductDTO productDTO)
        {
            if (id != productDTO.ProductId)
                return BadRequest($"Id={id} does not match ProductId={productDTO.ProductId}...");

            var product = _mapper.Map<Product>(productDTO);
            var updatedProduct = _uof.ProductRepository.Update(product);
            _uof.Commit();

            var updatedProductDTO = _mapper.Map<ProductDTO>(updatedProduct);

            return updatedProductDTO;
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProductDTO> Delete(int id)
        {
            var deletedProduct = _uof.ProductRepository.Delete(id);
            _uof.Commit();

            var deletedProductDTO = _mapper.Map<ProductDTO>(deletedProduct);

            return deletedProductDTO;
        }
    }
}
