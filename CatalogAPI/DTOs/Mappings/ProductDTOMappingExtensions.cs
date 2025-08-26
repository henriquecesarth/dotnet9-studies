using CatalogAPI.Models;

namespace CatalogAPI.DTOs.Mappings;

public static class ProductDTOMappingExtensions
{
    public static ProductDTO MapToProductDTO(this Product product)
    {
        return new ProductDTO
        {
            ProductId = product.ProductId,
            Name = product.Name,
            ImgUrl = product.ImgUrl,
        };
    }

    public static Product MapToProduct(this ProductDTO productDTO)
    {
        return new Product
        {
            ProductId = productDTO.ProductId,
            Name = productDTO.Name,
            ImgUrl = productDTO.ImgUrl,
        };
    }

    public static IEnumerable<ProductDTO> MapToProductDTOList(this IEnumerable<Product> products)
    {
        return products.Select(x => x.MapToProductDTO());
    }
}
