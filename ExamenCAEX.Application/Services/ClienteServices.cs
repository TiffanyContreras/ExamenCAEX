using System;
using System.Collections.Generic;
using System.Text;


using ExamenCAEX.Application.DTOs.Clientes;
using ExamenCAEX.Application.Interfaces;
using ExamenCAEX.Application.Repositories;
using ExamenCAEX.Domain.Entities;

namespace ExamenCAEX.Application.Services;

public sealed class ClienteService : IClienteService
{
    private readonly IClienteRepository _repository;

    public ClienteService(IClienteRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<ClienteDto>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default)
    {
        var clientes = await _repository
            .ObtenerTodosAsync(cancellationToken);

        return clientes.Select(MapearDto).ToList();
    }

    public async Task<ClienteDto?> ObtenerPorIdAsync(
        int clienteId,
        CancellationToken cancellationToken = default)
    {
        var cliente = await _repository
            .ObtenerPorIdAsync(clienteId, cancellationToken);

        return cliente is null ? null : MapearDto(cliente);
    }

    public async Task<int> CrearAsync(
        CrearClienteDto dto,
        CancellationToken cancellationToken = default)
    {
        var cliente = new Cliente(
            dto.Nombre,
            dto.NIT,
            dto.Email,
            dto.Telefono,
            dto.Direccion);

        if (await _repository.ExisteNitAsync(
            cliente.NIT,
            cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException(
                "Ya existe un cliente con ese NIT.");
        }

        await _repository.AgregarAsync(
            cliente, cancellationToken);

        await _repository.GuardarCambiosAsync(
            cancellationToken);

        return cliente.ClienteId;
    }

    public async Task ActualizarAsync(
        int clienteId,
        CrearClienteDto dto,
        CancellationToken cancellationToken = default)
    {
        var cliente = await _repository
            .ObtenerPorIdAsync(clienteId, cancellationToken)
            ?? throw new KeyNotFoundException(
                "El cliente no existe.");

        if (await _repository.ExisteNitAsync(
            dto.NIT.Trim(),
            excluirClienteId: clienteId,
            cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException(
                "Ya existe otro cliente con ese NIT.");
        }

        cliente.Actualizar(
            dto.Nombre,
            dto.NIT,
            dto.Email,
            dto.Telefono,
            dto.Direccion);

        await _repository.GuardarCambiosAsync(
            cancellationToken);
    }

    public async Task DesactivarAsync(
        int clienteId,
        CancellationToken cancellationToken = default)
    {
        var cliente = await _repository
            .ObtenerPorIdAsync(clienteId, cancellationToken)
            ?? throw new KeyNotFoundException(
                "El cliente no existe.");

        cliente.Desactivar();

        await _repository.GuardarCambiosAsync(
            cancellationToken);
    }

    private static ClienteDto MapearDto(Cliente cliente)
    {
        return new ClienteDto(
            cliente.ClienteId,
            cliente.Nombre,
            cliente.NIT,
            cliente.Email,
            cliente.Telefono,
            cliente.Direccion,
            cliente.Activo);
    }
}
