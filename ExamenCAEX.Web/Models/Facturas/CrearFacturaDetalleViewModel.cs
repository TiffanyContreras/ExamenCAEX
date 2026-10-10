
using System.ComponentModel.DataAnnotations;

namespace ExamenCAEX.Web.Models.Facturas;

public class CrearFacturaDetalleViewModel
{
    [Required(ErrorMessage = "Seleccione un producto.")]
    [Range(1, int.MaxValue,
        ErrorMessage = "Seleccione un producto válido.")]
    public int ProductoId { get; set; }

    [Required(ErrorMessage = "Ingrese la cantidad.")]
    [Range(1, int.MaxValue,
        ErrorMessage = "La cantidad debe ser mayor que cero.")]
    public int Cantidad { get; set; } = 1;
}

