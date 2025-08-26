using CatalogAPI.DTOs;
using CatalogAPI.DTOs.Mappings;
using CatalogAPI.Models;
using CatalogAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _uof;

        public ProductsController(IUnitOfWork uof)
        {
            _uof = uof;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDTO>> Get()
        {
            var products = _uof.ProductRepository.GetAll();

            if (products == null)
                return NotFound("Any product was found...");

            var productsDTO = products.MapToProductDTOList();

            return Ok(productsDTO);
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public ActionResult<ProductDTO> Get(int id)
        {
            var product = _uof.ProductRepository.Get(p => p.ProductId == id);

            if (product == null)
                return NotFound($"Product with id={id} not found...");

            var productDTO = product.MapToProductDTO();

            return productDTO;
        }

        [HttpGet("Categories/{categoryId:int}")]
        public ActionResult<IEnumerable<ProductDTO>> GetProductsByCategory(int categoryId)
        {
            var products = _uof.ProductRepository.GetProductsByCategory(categoryId);

            if (products == null || !products.Any())
                return NotFound($"Any product was found for category with id={categoryId}...");

            var productsDTO = products.MapToProductDTOList();

            return Ok(productsDTO);
        }

        [HttpPost]
        public ActionResult<ProductDTO> Post(ProductDTO productDTO)
        {
            var product = productDTO.MapToProduct();

            var createdProduct = _uof.ProductRepository.Create(product);
            _uof.Commit();

            var createdProductDTO = createdProduct.MapToProductDTO();

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

            var product = productDTO.MapToProduct();
            var updatedProduct = _uof.ProductRepository.Update(product);
            _uof.Commit();

            var updatedProductDTO = updatedProduct.MapToProductDTO();

            return updatedProductDTO;
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProductDTO> Delete(int id)
        {
            var deletedProduct = _uof.ProductRepository.Delete(id);
            _uof.Commit();

            var deletedProductDTO = deletedProduct.MapToProductDTO();

            return deletedProductDTO;
        }
    }
}
