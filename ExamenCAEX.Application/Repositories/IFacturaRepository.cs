using System;
using System.Collections.Generic;
using System.Text;

using ExamenCAEX.Domain.Entities;

namespace ExamenCAEX.Application.Repositories;

public interface IFacturaRepository
{
    Task<IReadOnlyList<Factura>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default);

    Task<Factura?> ObtenerPorIdAsync(
        int facturaId,
        CancellationToken cancellationToken = default);

    Task<EstadoFactura?> ObtenerEstadoPorCodigoAsync(
        string codigo,
        CancellationToken cancellationToken = default);

    Task AgregarAsync(
        Factura factura,
        CancellationToken cancellationToken = default);

    Task GuardarCambiosAsync(
        CancellationToken cancellationToken = default);

    Task<string> GenerarNumeroFacturaAsync(
        CancellationToken cancellationToken = default);
}
