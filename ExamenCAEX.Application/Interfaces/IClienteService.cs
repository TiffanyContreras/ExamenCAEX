
using ExamenCAEX.Application.DTOs.Clientes;

namespace ExamenCAEX.Application.Interfaces;

public interface IClienteService
{
   
    Task<IReadOnlyList<ClienteDto>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default);

   
    Task<ClienteDto?> ObtenerPorIdAsync(
        int clienteId,
        CancellationToken cancellationToken = default);

    
    Task<int> CrearAsync(
        CrearClienteDto dto,
        CancellationToken cancellationToken = default);

   
    Task ActualizarAsync(
        int clienteId,
        CrearClienteDto dto,
        CancellationToken cancellationToken = default);

   
    Task DesactivarAsync(
        int clienteId,
        CancellationToken cancellationToken = default);
}
