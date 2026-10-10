
using System.ComponentModel.DataAnnotations;

namespace ExamenCAEX.Web.Models.Clientes;

public sealed class EditarClienteViewModel
{
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(150)]
    [Display(Name = "Nombre completo")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El NIT es obligatorio.")]
    [StringLength(20)]
    public string NIT { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "El correo no es válido.")]
    [StringLength(150)]
    public string? Email { get; set; }

    [StringLength(20)]
    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }

    [StringLength(250)]
    [Display(Name = "Dirección")]
    public string? Direccion { get; set; }
}

