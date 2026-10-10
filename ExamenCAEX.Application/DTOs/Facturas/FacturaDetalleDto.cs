using System;
using System.Collections.Generic;
using System.Text;


namespace ExamenCAEX.Application.DTOs.Facturas;

public sealed record FacturaDetalleDto(
    int FacturaDetalleId,
    int ProductoId,
    string CodigoProducto,
    string NombreProducto,
    int Cantidad,
    decimal PrecioUnitario,
    decimal Subtotal
);

