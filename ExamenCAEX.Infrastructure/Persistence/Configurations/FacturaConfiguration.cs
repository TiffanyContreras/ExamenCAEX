using System;
using System.Collections.Generic;
using System.Text;


using ExamenCAEX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamenCAEX.Infrastructure.Persistence.Configurations;

public sealed class FacturaConfiguration
    : IEntityTypeConfiguration<Factura>
{
    public void Configure(
        EntityTypeBuilder<Factura> builder)
    {
        // Tabla existente
        builder.ToTable("Facturas", "examen");

        // Clave primaria
        builder.HasKey(f => f.FacturaId);

        builder.Property(f => f.FacturaId)
            .ValueGeneratedOnAdd();

        // Número de factura
        builder.Property(f => f.NumeroFactura)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(30);

        builder.HasAlternateKey(f => f.NumeroFactura)
            .HasName("UQ_Facturas_Numero");

        // Cliente
        builder.Property(f => f.ClienteId)
            .IsRequired();

        // Estado
        builder.Property(f => f.EstadoFacturaId)
            .IsRequired();

        // Fecha de emisión
        builder.Property(f => f.FechaEmision)
            .HasColumnType("datetime2")
            .HasDefaultValueSql("SYSUTCDATETIME()");

        // Total
        builder.Property(f => f.Total)
            .IsRequired()
            .HasPrecision(18, 2);

        // Anulación
        builder.Property(f => f.FechaAnulacion)
            .HasColumnType("datetime2");

        builder.Property(f => f.MotivoAnulacion)
            .HasMaxLength(250);

        // Relación Factura -> Cliente
        builder.HasOne<Cliente>()
            .WithMany()
            .HasForeignKey(f => f.ClienteId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_Facturas_Clientes");

        // Relación Factura -> EstadoFactura
        builder.HasOne(f => f.Estado)
            .WithMany()
            .HasForeignKey(f => f.EstadoFacturaId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_Facturas_Estados");

        // Relación Factura -> FacturaDetalle
        builder.HasMany(f => f.Detalles)
            .WithOne()
            .HasForeignKey(d => d.FacturaId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_FacturaDetalles_Facturas");

        // Utilizar el campo privado de la colección
        builder.Navigation(f => f.Detalles)
            .HasField("_detalles")
            .UsePropertyAccessMode(
                PropertyAccessMode.Field);
    }
}
