using System;
using System.Collections.Generic;
using System.Text;

using ExamenCAEX.Domain.Entities;
using ExamenCAEX.Application.DTOs.Facturas;
using ExamenCAEX.Application.Interfaces;
using ExamenCAEX.Application.Repositories;

namespace ExamenCAEX.Application.Services;

public sealed class FacturaService : IFacturaService
{
    private readonly IFacturaRepository _facturaRepository;
    private readonly IClienteService _clienteService;
    private readonly IProductoService _productoService;

    public FacturaService(
        IFacturaRepository facturaRepository,
        IClienteService clienteService,
        IProductoService productoService)
    {
        _facturaRepository = facturaRepository;
        _clienteService = clienteService;
        _productoService = productoService;
    }


    public async Task<int> CrearAsync(
        CrearFacturaDto dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

      
        var cliente = await _clienteService.ObtenerPorIdAsync(
            dto.ClienteId,
            cancellationToken);

        if (cliente is null || !cliente.Activo)
        {
            throw new InvalidOperationException(
                "El cliente no existe o está inactivo.");
        }

        
        if (dto.Detalles is null || dto.Detalles.Count == 0)
        {
            throw new InvalidOperationException(
                "La factura debe contener al menos un producto.");
        }

       
        if (dto.Detalles
            .GroupBy(d => d.ProductoId)
            .Any(g => g.Count() > 1))
        {
            throw new InvalidOperationException(
                "No se puede agregar el mismo producto dos veces.");
        }

       
        var detallesValidados =
            new List<(int ProductoId, int Cantidad, decimal Precio)>();

        foreach (var detalle in dto.Detalles)
        {
            if (detalle.Cantidad <= 0)
            {
                throw new InvalidOperationException(
                    "La cantidad debe ser mayor que cero.");
            }

            var producto = await _productoService.ObtenerPorIdAsync(
                detalle.ProductoId,
                cancellationToken);

            if (producto is null || !producto.Activo)
            {
                throw new InvalidOperationException(
                    $"El producto {detalle.ProductoId} no existe o está inactivo.");
            }

            detallesValidados.Add((
                producto.ProductoId,
                detalle.Cantidad,
                producto.Precio
            ));
        }

       
        var estado = await _facturaRepository
            .ObtenerEstadoPorCodigoAsync(
                "EMITIDA",
                cancellationToken);

        if (estado is null)
        {
            throw new InvalidOperationException(
                "No se encontró el estado EMITIDA.");
        }

      
        var numeroFactura = await _facturaRepository
            .GenerarNumeroFacturaAsync(cancellationToken);

      
        var factura = new Factura(
            numeroFactura,
            dto.ClienteId,
            estado.EstadoFacturaId);

       
        foreach (var detalle in detallesValidados)
        {
            factura.AgregarDetalle(
                detalle.ProductoId,
                detalle.Cantidad,
                detalle.Precio);
        }

       
        factura.ValidarParaEmitir();

     
        await _facturaRepository.AgregarAsync(
            factura,
            cancellationToken);

        await _facturaRepository.GuardarCambiosAsync(
            cancellationToken);

       
        return factura.FacturaId;
    }




    public async Task<IReadOnlyList<FacturaDto>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default)
    {
        var facturas = await _facturaRepository.ObtenerTodosAsync(
            cancellationToken);

        var resultado = new List<FacturaDto>();

        foreach (var factura in facturas)
        {
            var cliente = await _clienteService.ObtenerPorIdAsync(
                factura.ClienteId,
                cancellationToken);

            resultado.Add(new FacturaDto(
                factura.FacturaId,
                factura.NumeroFactura,
                factura.ClienteId,
                cliente?.Nombre ?? "Cliente no disponible",
                factura.Estado.Codigo,
                factura.FechaEmision,
                factura.Total,
                factura.FechaAnulacion,
                factura.MotivoAnulacion,
                Array.Empty<FacturaDetalleDto>()
            ));
        }

        return resultado;
    }



    public async Task<FacturaDto?> ObtenerPorIdAsync(
        int facturaId,
        CancellationToken cancellationToken = default)
    {
        
        var factura = await _facturaRepository.ObtenerPorIdAsync(
            facturaId,
            cancellationToken);

        if (factura is null)
        {
            return null;
        }

       
        var cliente = await _clienteService.ObtenerPorIdAsync(
            factura.ClienteId,
            cancellationToken);

       
        var detalles = new List<FacturaDetalleDto>();

        foreach (var detalle in factura.Detalles)
        {
            var producto = await _productoService.ObtenerPorIdAsync(
                detalle.ProductoId,
                cancellationToken);

            detalles.Add(new FacturaDetalleDto(
                detalle.FacturaDetalleId,
                detalle.ProductoId,
                producto?.Codigo ?? "N/D",
                producto?.Nombre ?? "Producto no disponible",
                detalle.Cantidad,
                detalle.PrecioUnitario,
                detalle.Subtotal
            ));
        }

        
        return new FacturaDto(
            factura.FacturaId,
            factura.NumeroFactura,
            factura.ClienteId,
            cliente?.Nombre ?? "Cliente no disponible",
            factura.Estado.Codigo,
            factura.FechaEmision,
            factura.Total,
            factura.FechaAnulacion,
            factura.MotivoAnulacion,
            detalles
        );
    }



    public async Task AnularAsync(
        int facturaId,
        string motivo,
        CancellationToken cancellationToken = default)
    {
       
        if (facturaId <= 0)
        {
            throw new InvalidOperationException(
                "El ID de la factura no es válido.");
        }

       
        if (string.IsNullOrWhiteSpace(motivo))
        {
            throw new InvalidOperationException(
                "Debe indicar el motivo de anulación.");
        }

        motivo = motivo.Trim();

      
        var factura = await _facturaRepository.ObtenerPorIdAsync(
            facturaId,
            cancellationToken);

        if (factura is null)
        {
            throw new InvalidOperationException(
                "La factura no existe.");
        }

       
        if (factura.Estado.Codigo != "EMITIDA")
        {
            throw new InvalidOperationException(
                "Solo se pueden anular facturas emitidas.");
        }

      
        var estadoAnulada = await _facturaRepository
            .ObtenerEstadoPorCodigoAsync(
                "ANULADA",
                cancellationToken);

        if (estadoAnulada is null)
        {
            throw new InvalidOperationException(
                "No se encontró el estado ANULADA.");
        }

       
        factura.Anular(
            motivo,
            estadoAnulada.EstadoFacturaId);

       
        await _facturaRepository.GuardarCambiosAsync(
            cancellationToken);
    }

}
