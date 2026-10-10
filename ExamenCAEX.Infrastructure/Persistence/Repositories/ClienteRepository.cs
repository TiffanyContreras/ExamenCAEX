using System;
using System.Collections.Generic;
using System.Text;

using ExamenCAEX.Application.Repositories;
using ExamenCAEX.Domain.Entities;
using ExamenCAEX.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExamenCAEX.Infrastructure.Persistence.Repositories;

public sealed class ClienteRepository : IClienteRepository
{
    private readonly ExamenDbContext _context;

    public ClienteRepository(ExamenDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Cliente>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .AsNoTracking()
            .OrderBy(c => c.Nombre)
            .ToListAsync(cancellationToken);
    }

    public async Task<Cliente?> ObtenerPorIdAsync(
        int clienteId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(
                c => c.ClienteId == clienteId,
                cancellationToken);
    }

    public async Task<bool> ExisteNitAsync(
        string nit,
        int? excluirClienteId = null,
        CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .AsNoTracking()
            .AnyAsync(
                c => c.NIT == nit &&
                    (!excluirClienteId.HasValue ||
                     c.ClienteId != excluirClienteId.Value),
                cancellationToken);
    }

    public async Task AgregarAsync(
        Cliente cliente,
        CancellationToken cancellationToken = default)
    {
        await _context.Clientes.AddAsync(
            cliente,
            cancellationToken);
    }

    public async Task GuardarCambiosAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
