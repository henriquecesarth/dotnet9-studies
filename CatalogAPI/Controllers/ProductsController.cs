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
        public ActionResult<IEnumerable<Product>> Get()
        {
            var products = _uof.ProductRepository.GetAll();

            if (products == null)
                return NotFound("Any product was found...");

            return Ok(products);
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public ActionResult<Product> Get(int id)
        {
            var product = _uof.ProductRepository.Get(p => p.ProductId == id);

            if (product == null)
                return NotFound($"Product with id={id} not found...");

            return product;
        }

        [HttpGet("Categories/{categoryId:int}")]
        public ActionResult<IEnumerable<Product>> GetProductsByCategory(int categoryId)
        {
            var products = _uof.ProductRepository.GetProductsByCategory(categoryId);

            if (products == null || !products.Any())
                return NotFound($"Any product was found for category with id={categoryId}...");

            return Ok(products);
        }

        [HttpPost]
        public ActionResult<Product> Post(Product product)
        {
            var createdProduct = _uof.ProductRepository.Create(product);
            _uof.Commit();

            return new CreatedAtRouteResult(
                "GetProduct",
                new { id = createdProduct.ProductId },
                createdProduct
            );
        }

        [HttpPut("{id:int}")]
        public ActionResult<Product> Put(int id, Product product)
        {
            if (id != product.ProductId)
                return BadRequest($"Id={id} does not match ProductId={product.ProductId}...");

            var updatedProduct = _uof.ProductRepository.Update(product);
            _uof.Commit();

            return updatedProduct;
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Product> Delete(int id)
        {
            var deletedProduct = _uof.ProductRepository.Delete(id);
            _uof.Commit();

            return deletedProduct;
        }
    }
}
