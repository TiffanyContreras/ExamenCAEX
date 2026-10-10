
using ExamenCAEX.Application.DTOs.Productos;
using ExamenCAEX.Application.Interfaces;
using ExamenCAEX.Application.Repositories;
using ExamenCAEX.Domain.Entities;

namespace ExamenCAEX.Application.Services;

public sealed class ProductoService : IProductoService
{
    private readonly IProductoRepository _repository;

    public ProductoService(IProductoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<ProductoDto>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default)
    {
        var productos = await _repository
            .ObtenerTodosAsync(cancellationToken);

        return productos.Select(Mapear).ToList();
    }

    public async Task<ProductoDto?> ObtenerPorIdAsync(
        int productoId,
        CancellationToken cancellationToken = default)
    {
        var producto = await _repository
            .ObtenerPorIdAsync(productoId, cancellationToken);

        return producto is null ? null : Mapear(producto);
    }

    public async Task<int> CrearAsync(
        CrearProductoDto dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var codigo = dto.Codigo?.Trim() ?? string.Empty;

        if (await _repository.ExisteCodigoAsync(
            codigo, cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException(
                "Ya existe un producto con ese código.");
        }

        var producto = new Producto(
            codigo,
            dto.Nombre,
            dto.Descripcion,
            dto.Precio);

        await _repository.AgregarAsync(
            producto, cancellationToken);

        await _repository.GuardarCambiosAsync(
            cancellationToken);

        return producto.ProductoId;
    }

    public async Task ActualizarAsync(
        int productoId,
        CrearProductoDto dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var producto = await _repository
            .ObtenerPorIdAsync(productoId, cancellationToken);

        if (producto is null)
        {
            throw new InvalidOperationException(
                "El producto no existe.");
        }

        var codigo = dto.Codigo?.Trim() ?? string.Empty;

        if (await _repository.ExisteCodigoAsync(
            codigo,
            productoId,
            cancellationToken))
        {
            throw new InvalidOperationException(
                "Ya existe otro producto con ese código.");
        }

        producto.Actualizar(
            codigo,
            dto.Nombre,
            dto.Descripcion,
            dto.Precio);

        await _repository.GuardarCambiosAsync(
            cancellationToken);
    }

    public async Task DesactivarAsync(
        int productoId,
        CancellationToken cancellationToken = default)
    {
        var producto = await _repository
            .ObtenerPorIdAsync(productoId, cancellationToken);

        if (producto is null)
        {
            throw new InvalidOperationException(
                "El producto no existe.");
        }

        if (!producto.Activo)
        {
            throw new InvalidOperationException(
                "El producto ya está inactivo.");
        }

        producto.Desactivar();

        await _repository.GuardarCambiosAsync(
            cancellationToken);
    }

    private static ProductoDto Mapear(Producto producto)
    {
        return new ProductoDto(
            producto.ProductoId,
            producto.Codigo,
            producto.Nombre,
            producto.Descripcion,
            producto.Precio,
            producto.Activo);
    }
}

