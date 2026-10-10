
using ExamenCAEX.Application.DTOs.Productos;

namespace ExamenCAEX.Application.Interfaces;

public interface IProductoService
{
    // Obtener todos los productos
    Task<IReadOnlyList<ProductoDto>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default);

    // Obtener un producto por su ID
    Task<ProductoDto?> ObtenerPorIdAsync(
        int productoId,
        CancellationToken cancellationToken = default);

    // Registrar un nuevo producto
    Task<int> CrearAsync(
        CrearProductoDto dto,
        CancellationToken cancellationToken = default);

    // Actualizar un producto existente
    Task ActualizarAsync(
        int productoId,
        CrearProductoDto dto,
        CancellationToken cancellationToken = default);

    // Desactivar un producto
    Task DesactivarAsync(
        int productoId,
        CancellationToken cancellationToken = default);
}
