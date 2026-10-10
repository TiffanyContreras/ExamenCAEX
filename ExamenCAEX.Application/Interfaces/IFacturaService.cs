using System;
using System.Collections.Generic;
using System.Text;

using ExamenCAEX.Application.DTOs.Facturas;

namespace ExamenCAEX.Application.Interfaces;

public interface IFacturaService
{
    Task<IReadOnlyList<FacturaDto>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default);

    Task<FacturaDto?> ObtenerPorIdAsync(
        int facturaId,
        CancellationToken cancellationToken = default);

    Task<int> CrearAsync(
        CrearFacturaDto dto,
        CancellationToken cancellationToken = default);

    Task AnularAsync(
        int facturaId,
        string motivo,
        CancellationToken cancellationToken = default);
}
