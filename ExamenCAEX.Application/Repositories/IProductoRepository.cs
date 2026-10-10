
using ExamenCAEX.Domain.Entities;

namespace ExamenCAEX.Application.Repositories;

public interface IProductoRepository
{
    Task<IReadOnlyList<Producto>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default);

    Task<Producto?> ObtenerPorIdAsync(
        int productoId,
        CancellationToken cancellationToken = default);

    Task<bool> ExisteCodigoAsync(
        string codigo,
        int? excluirProductoId = null,
        CancellationToken cancellationToken = default);

    Task AgregarAsync(
        Producto producto,
        CancellationToken cancellationToken = default);

    Task GuardarCambiosAsync(
        CancellationToken cancellationToken = default);
}

