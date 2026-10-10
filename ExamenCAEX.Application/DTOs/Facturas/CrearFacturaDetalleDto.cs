using System;
using System.Collections.Generic;
using System.Text;


namespace ExamenCAEX.Application.DTOs.Facturas;

public sealed record CrearFacturaDetalleDto(
    int ProductoId,
    int Cantidad
);
