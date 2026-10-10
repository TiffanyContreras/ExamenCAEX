
using ExamenCAEX.Application.Repositories;
using ExamenCAEX.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamenCAEX.Infrastructure.Persistence.Repositories;

public sealed class ProductoRepository : IProductoRepository
{
    private readonly ExamenDbContext _context;

    public ProductoRepository(ExamenDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Producto>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Productos
            .AsNoTracking()
            .OrderBy(p => p.Nombre)
            .ToListAsync(cancellationToken);
    }

    public async Task<Producto?> ObtenerPorIdAsync(
        int productoId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Productos
            .FirstOrDefaultAsync(
                p => p.ProductoId == productoId,
                cancellationToken);
    }

    public async Task<bool> ExisteCodigoAsync(
        string codigo,
        int? excluirProductoId = null,
        CancellationToken cancellationToken = default)
    {
        return await _context.Productos
            .AsNoTracking()
            .AnyAsync(
                p => p.Codigo == codigo &&
                    (!excluirProductoId.HasValue ||
                     p.ProductoId != excluirProductoId.Value),
                cancellationToken);
    }

    public async Task AgregarAsync(
        Producto producto,
        CancellationToken cancellationToken = default)
    {
        await _context.Productos.AddAsync(
            producto,
            cancellationToken);
    }

    public async Task GuardarCambiosAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
