using System;
using System.Collections.Generic;
using System.Text;




namespace ExamenCAEX.Domain.Entities;

public class FacturaDetalle
{
    public int FacturaDetalleId { get; private set; }

    public int FacturaId { get; private set; }

    public int ProductoId { get; private set; }

    public int Cantidad { get; private set; }

    public decimal PrecioUnitario { get; private set; }

    public decimal Subtotal { get; private set; }

    // Constructor para Entity Framework Core
    private FacturaDetalle() { }

    public FacturaDetalle(
        int productoId,
        int cantidad,
        decimal precioUnitario)
    {
        if (productoId <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(productoId),
                "El producto debe ser válido.");

        if (cantidad <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(cantidad),
                "La cantidad debe ser mayor a cero.");

        if (precioUnitario < 0 ||
            decimal.Round(precioUnitario, 2) != precioUnitario)
            throw new ArgumentOutOfRangeException(
                nameof(precioUnitario),
                "El precio debe ser positivo o cero y tener máximo dos decimales.");

        if (precioUnitario > 9999999999999999.99m / cantidad)
            throw new ArgumentOutOfRangeException(
                nameof(cantidad),
                "El subtotal excede el límite permitido.");

        ProductoId = productoId;
        Cantidad = cantidad;
        PrecioUnitario = precioUnitario;
    }

    public decimal CalcularSubtotal()
    {
        return checked(Cantidad * PrecioUnitario);
    }
}
