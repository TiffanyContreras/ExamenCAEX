using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using ExamenCAEX.Application.Repositories;
using ExamenCAEX.Domain.Entities;
using ExamenCAEX.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExamenCAEX.Infrastructure.Persistence.Repositories;

public sealed class FacturaRepository : IFacturaRepository
{
    private readonly ExamenDbContext _context;

    public FacturaRepository(ExamenDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Factura>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Facturas
            .AsNoTracking()
            .Include(f => f.Estado)
            .OrderByDescending(f => f.FechaEmision)
            .ToListAsync(cancellationToken);
    }

    public async Task<Factura?> ObtenerPorIdAsync(
        int facturaId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Facturas
            .Include(f => f.Estado)
            .Include(f => f.Detalles)
            .FirstOrDefaultAsync(
                f => f.FacturaId == facturaId,
                cancellationToken);
    }

    public async Task<EstadoFactura?> ObtenerEstadoPorCodigoAsync(
        string codigo,
        CancellationToken cancellationToken = default)
    {
        return await _context.EstadosFactura
            .AsNoTracking()
            .FirstOrDefaultAsync(
                e => e.Codigo == codigo,
                cancellationToken);
    }

    public async Task AgregarAsync(
        Factura factura,
        CancellationToken cancellationToken = default)
    {
        await _context.Facturas.AddAsync(
            factura,
            cancellationToken);
    }

    public async Task GuardarCambiosAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(
            cancellationToken);
    }


    public async Task<string> GenerarNumeroFacturaAsync(
        CancellationToken cancellationToken = default)
    {
        var conexion = _context.Database.GetDbConnection();

        var debeCerrarConexion =
            conexion.State != ConnectionState.Open;

        if (debeCerrarConexion)
        {
            await conexion.OpenAsync(cancellationToken);
        }

        try
        {
            await using var comando = conexion.CreateCommand();

            comando.CommandText =
                "SELECT NEXT VALUE FOR examen.SecuenciaFacturas";

            var resultado = await comando.ExecuteScalarAsync(
                cancellationToken);

            var numero = Convert.ToInt64(resultado);

            return $"FAC-{numero:D6}";
        }
        finally
        {
            if (debeCerrarConexion)
            {
                await conexion.CloseAsync();
            }
        }
    }

}
