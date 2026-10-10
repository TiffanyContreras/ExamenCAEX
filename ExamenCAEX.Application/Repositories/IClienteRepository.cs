using System;
using System.Collections.Generic;
using System.Text;


using ExamenCAEX.Domain.Entities;

namespace ExamenCAEX.Application.Repositories;

public interface IClienteRepository
{
    Task<IReadOnlyList<Cliente>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default);

    Task<Cliente?> ObtenerPorIdAsync(
        int clienteId,
        CancellationToken cancellationToken = default);

    Task<bool> ExisteNitAsync(
        string nit,
        int? excluirClienteId = null,
        CancellationToken cancellationToken = default);

    Task AgregarAsync(
        Cliente cliente,
        CancellationToken cancellationToken = default);

    Task GuardarCambiosAsync(
        CancellationToken cancellationToken = default);
}
