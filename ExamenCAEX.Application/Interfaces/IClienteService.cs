
using ExamenCAEX.Application.DTOs.Clientes;

namespace ExamenCAEX.Application.Interfaces;

public interface IClienteService
{
    // Obtener todos los clientes
    Task<IReadOnlyList<ClienteDto>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default);

    // Obtener un cliente por su ID
    Task<ClienteDto?> ObtenerPorIdAsync(
        int clienteId,
        CancellationToken cancellationToken = default);

    // Registrar un nuevo cliente
    Task<int> CrearAsync(
        CrearClienteDto dto,
        CancellationToken cancellationToken = default);

    // Actualizar un cliente existente
    Task ActualizarAsync(
        int clienteId,
        CrearClienteDto dto,
        CancellationToken cancellationToken = default);

    // Desactivar un cliente
    Task DesactivarAsync(
        int clienteId,
        CancellationToken cancellationToken = default);
}
