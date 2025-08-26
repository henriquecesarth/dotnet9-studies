using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CatalogAPI.Models;

namespace CatalogAPI.DTOs;

public class CategoryDTO
{
    [Required(ErrorMessage = "The CategoryId is required")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "The name is required")]
    [StringLength(
        80,
        MinimumLength = 5,
        ErrorMessage = "The name must be between {2} and {1} characters long"
    )]
    public string? Name { get; set; }

    [Required(ErrorMessage = "The image url is required")]
    [StringLength(300, ErrorMessage = "The image url must be at maximum {1} characters long")]
    public string? ImgUrl { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ICollection<Product>? Products { get; set; }
}
