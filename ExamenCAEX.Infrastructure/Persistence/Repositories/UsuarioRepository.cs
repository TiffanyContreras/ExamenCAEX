
using ExamenCAEX.Application.Repositories;
using ExamenCAEX.Domain.Entities;
using ExamenCAEX.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExamenCAEX.Infrastructure.Repositories;

public sealed class UsuarioRepository : IUsuarioRepository
{
    private readonly ExamenDbContext _context;

    public UsuarioRepository(ExamenDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObtenerPorNombreAsync(
        string nombreUsuario,
        CancellationToken cancellationToken = default)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(
                u => u.NombreUsuario == nombreUsuario,
                cancellationToken);
    }

    public async Task AgregarAsync(
        Usuario usuario,
        CancellationToken cancellationToken = default)
    {
        await _context.Usuarios.AddAsync(
            usuario,
            cancellationToken);
    }

    public async Task GuardarCambiosAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
