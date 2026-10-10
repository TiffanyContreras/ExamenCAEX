using System;
using System.Collections.Generic;
using System.Text;


using ExamenCAEX.Application.DTOs.Reportes;
using ExamenCAEX.Application.Interfaces;
using ExamenCAEX.Application.Repositories;

namespace ExamenCAEX.Application.Services;

public sealed class ReporteService : IReporteService
{
    private readonly IFacturaRepository _facturaRepository;

    public ReporteService(
        IFacturaRepository facturaRepository)
    {
        _facturaRepository = facturaRepository;
    }

    public async Task<ResumenFacturasDto> ObtenerResumenFacturasAsync(
        DateTime? fechaInicio = null,
        DateTime? fechaFin = null,
        CancellationToken cancellationToken = default)
    {
        if (fechaInicio.HasValue &&
            fechaFin.HasValue &&
            fechaInicio.Value > fechaFin.Value)
        {
            throw new ArgumentException(
                "La fecha inicial no puede ser mayor que la fecha final.");
        }

        var facturas = await _facturaRepository.ObtenerTodosAsync(
            cancellationToken);


        var filtradas = facturas.Where(f =>
            (!fechaInicio.HasValue ||
             f.FechaEmision >= fechaInicio.Value) &&
            (!fechaFin.HasValue ||
             f.FechaEmision < fechaFin.Value)
        ).ToList();


        int total = filtradas.Count;

        int emitidas = filtradas.Count(f =>
            f.Estado.Codigo == "EMITIDA");

        int anuladas = filtradas.Count(f =>
            f.Estado.Codigo == "ANULADA");

        decimal montoEmitidas = filtradas
            .Where(f => f.Estado.Codigo == "EMITIDA")
            .Sum(f => f.Total);

        return new ResumenFacturasDto(
            total,
            emitidas,
            anuladas,
            montoEmitidas
        );
    }
}
