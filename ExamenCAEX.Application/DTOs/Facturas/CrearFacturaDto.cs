using System;
using System.Collections.Generic;
using System.Text;


namespace ExamenCAEX.Application.DTOs.Facturas;

public sealed record CrearFacturaDto(
    int ClienteId,
    IReadOnlyList<CrearFacturaDetalleDto> Detalles
);
