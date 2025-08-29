using System;
using CatalogAPI.Models;

namespace CatalogAPI.DTOs;

public class CategoryProductsDTO : CategoryDTO
{
    public ICollection<ProductDTO> Products { get; set; }
}
