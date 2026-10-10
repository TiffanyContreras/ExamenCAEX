
using ExamenCAEX.Domain.Entities;

namespace ExamenCAEX.Application.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorNombreAsync(
        string nombreUsuario,
        CancellationToken cancellationToken = default);

    Task AgregarAsync(
        Usuario usuario,
        CancellationToken cancellationToken = default);

    Task GuardarCambiosAsync(
        CancellationToken cancellationToken = default);
}
