
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.ComponentModel.DataAnnotations;

namespace ExamenCAEX.Web.Models.Facturas;

public class CrearFacturaViewModel
{
    [Required(ErrorMessage = "Seleccione un cliente.")]
    [Range(1, int.MaxValue,
        ErrorMessage = "Seleccione un cliente válido.")]
    public int ClienteId { get; set; }

    public List<CrearFacturaDetalleViewModel> Detalles
    { get; set; } = new();

    
    [ValidateNever]
    public List<SelectListItem> Clientes
    { get; set; } = new();

    [ValidateNever]
    public List<SelectListItem> Productos
    { get; set; } = new();
}
