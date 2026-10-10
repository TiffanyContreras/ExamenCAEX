
using ExamenCAEX.Application.DTOs.Productos;

namespace ExamenCAEX.Application.Interfaces;

public interface IProductoService
{
  
    Task<IReadOnlyList<ProductoDto>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default);

 
    Task<ProductoDto?> ObtenerPorIdAsync(
        int productoId,
        CancellationToken cancellationToken = default);

    
    Task<int> CrearAsync(
        CrearProductoDto dto,
        CancellationToken cancellationToken = default);


    Task ActualizarAsync(
        int productoId,
        CrearProductoDto dto,
        CancellationToken cancellationToken = default);

   
    Task DesactivarAsync(
        int productoId,
        CancellationToken cancellationToken = default);
}
