using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatalogAPI.Models;

[Table("Products")]
public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "The name is required")]
    [StringLength(
        80,
        MinimumLength = 5,
        ErrorMessage = "The name must be between {2} and {1} characters long"
    )]
    public string? Name { get; set; }

    [Required(ErrorMessage = "The description is required")]
    [StringLength(300, ErrorMessage = "The description must be at maximum {1} characters long")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "The price is required")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "The image url is required")]
    [StringLength(300, ErrorMessage = "The image url must be at maximum {1} characters long")]
    public string? ImgUrl { get; set; }

    public float Stock { get; set; }

    public DateTime CreationDate { get; set; }

    public int CategoryId { get; set; }

    [JsonIgnore]
    public Category? Category { get; set; }
}
