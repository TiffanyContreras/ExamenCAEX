using System;
using System.Collections.Generic;
using System.Text;


using ExamenCAEX.Domain.Constants;

namespace ExamenCAEX.Domain.Entities;

public class Factura
{
    private readonly List<FacturaDetalle> _detalles = new();

    public int FacturaId { get; private set; }

    public string NumeroFactura { get; private set; } = string.Empty;

    public int ClienteId { get; private set; }

    public int EstadoFacturaId { get; private set; }

    public DateTime FechaEmision { get; private set; }

    public decimal Total { get; private set; }

    public DateTime? FechaAnulacion { get; private set; }

    public string? MotivoAnulacion { get; private set; }

    public EstadoFactura Estado { get; private set; } = null!;

    public IReadOnlyCollection<FacturaDetalle> Detalles =>
        _detalles.AsReadOnly();

    // Constructor para Entity Framework Core
    private Factura() { }

    public Factura(
        string numeroFactura,
        int clienteId,
        int estadoFacturaId)
    {
        if (string.IsNullOrWhiteSpace(numeroFactura) ||
            numeroFactura.Trim().Length > 30)
            throw new ArgumentException(
                "El número de factura es obligatorio y no debe superar 30 caracteres.",
                nameof(numeroFactura));

        if (clienteId <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(clienteId));

        if (estadoFacturaId <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(estadoFacturaId));

        NumeroFactura = numeroFactura.Trim();
        ClienteId = clienteId;
        EstadoFacturaId = estadoFacturaId;
        FechaEmision = DateTime.UtcNow;
        Total = 0;
    }

    public void AgregarDetalle(
        int productoId,
        int cantidad,
        decimal precioUnitario)
    {
        if (FechaAnulacion.HasValue)
            throw new InvalidOperationException(
                "No se pueden modificar facturas anuladas.");

        if (_detalles.Any(d => d.ProductoId == productoId))
            throw new InvalidOperationException(
                "El producto ya existe en la factura.");

        var detalle = new FacturaDetalle(
            productoId,
            cantidad,
            precioUnitario);

        decimal nuevoTotal = checked(Total + detalle.CalcularSubtotal());    
        
        if (nuevoTotal > 9999999999999999.99m)
            throw new InvalidOperationException(
                "El total excede el límite permitido.");

        _detalles.Add(detalle);
        Total = nuevoTotal;
    }

    public void ValidarParaEmitir()
    {
        if (_detalles.Count == 0)
            throw new InvalidOperationException(
                "La factura debe contener al menos un producto.");
    }

    public void Anular(
        string motivo,
        int estadoAnuladoId)
    {
        if (FechaAnulacion.HasValue ||
            Estado?.Codigo == EstadosFactura.Anulada)
            throw new InvalidOperationException(
                "La factura ya está anulada.");

        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException(
                "El motivo de anulación es obligatorio.",
                nameof(motivo));

        if (motivo.Trim().Length > 250)
            throw new ArgumentException(
                "El motivo no puede superar 250 caracteres.",
                nameof(motivo));

        if (estadoAnuladoId <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(estadoAnuladoId));

        EstadoFacturaId = estadoAnuladoId;
        FechaAnulacion = DateTime.UtcNow;
        MotivoAnulacion = motivo.Trim();
    }
}
