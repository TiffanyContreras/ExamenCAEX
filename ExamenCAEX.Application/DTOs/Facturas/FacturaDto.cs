using System;
using System.Collections.Generic;
using System.Text;


namespace ExamenCAEX.Application.DTOs.Facturas;

public sealed record FacturaDto(
    int FacturaId,
    string NumeroFactura,
    int ClienteId,
    string NombreCliente,
    string Estado,
    DateTime FechaEmision,
    decimal Total,
    DateTime? FechaAnulacion,
    string? MotivoAnulacion,
    IReadOnlyList<FacturaDetalleDto> Detalles
);
