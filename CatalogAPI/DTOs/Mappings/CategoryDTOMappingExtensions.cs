using System;
using CatalogAPI.Models;

namespace CatalogAPI.DTOs.Mappings;

public static class CategoryDTOMappingExtensions
{
    public static CategoryDTO MapToCategoryDTO(this Category category)
    {
        return new CategoryDTO
        {
            CategoryId = category.CategoryId,
            Name = category.Name,
            ImgUrl = category.ImgUrl,
            Products = category.Products,
        };
    }

    public static Category MapToCategory(this CategoryDTO categoryDTO)
    {
        return new Category
        {
            CategoryId = categoryDTO.CategoryId,
            Name = categoryDTO.Name,
            ImgUrl = categoryDTO.ImgUrl,
        };
    }

    public static IEnumerable<CategoryDTO> MapToCategoryDTOList(
        this IEnumerable<Category> categories
    )
    {
        return categories.Select(x => x.MapToCategoryDTO());
    }
}
