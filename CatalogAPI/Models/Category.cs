using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatalogAPI.Models;

[Table("Categories")]
public class Category
{
    public Category()
    {
        Products = new Collection<Product>();
    }

    [Key]
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

    [JsonIgnore]
    public ICollection<Product>? Products { get; set; }
}
