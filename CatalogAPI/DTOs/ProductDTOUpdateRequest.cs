using System;
using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.DTOs;

public class ProductDTOUpdateRequest : IValidatableObject
{
    [Range(1, 9999, ErrorMessage = "Stock must be between 1 and 9999")]
    public float Stock { get; set; }

    public DateTime CreationDate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CreationDate.Date < DateTime.Now.Date)
            yield return new ValidationResult(
                "Creation date must be after today",
                new[] { nameof(this.CreationDate) }
            );
    }
}
