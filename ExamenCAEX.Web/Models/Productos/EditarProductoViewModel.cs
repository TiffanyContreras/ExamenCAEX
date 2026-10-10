
using System.ComponentModel.DataAnnotations;

namespace ExamenCAEX.Web.Models.Productos;

public sealed class EditarProductoViewModel : IValidatableObject
{
    public int ProductoId { get; set; }

    [Required(ErrorMessage = "El código es obligatorio.")]
    [StringLength(30)]
    [Display(Name = "Código")]
    public string Codigo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(250)]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(typeof(decimal), "0", "9999999999999999.99",
        ErrorMessage = "El precio debe ser válido.")]
    public decimal? Precio { get; set; }


    public IEnumerable<ValidationResult> Validate(
        ValidationContext validationContext)
    {
        if (Precio.HasValue &&
            decimal.Round(Precio.Value, 2) != Precio.Value)
        {
            yield return new ValidationResult(
                "El precio debe tener como máximo dos decimales.",
                new[] { nameof(Precio) });
        }
    }

}

