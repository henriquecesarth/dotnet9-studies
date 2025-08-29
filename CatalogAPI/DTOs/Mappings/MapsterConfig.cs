using System;
using CatalogAPI.Models;
using Mapster;

namespace CatalogAPI.DTOs.Mappings;

public static class MapsterConfig
{
    public static void ConfigMapping()
    {
        TypeAdapterConfig<Category, CategoryDTO>.NewConfig().TwoWays();
        TypeAdapterConfig<Category, CategoryProductsDTO>.NewConfig().TwoWays();
        TypeAdapterConfig<Product, ProductDTO>.NewConfig().TwoWays();
    }
}
