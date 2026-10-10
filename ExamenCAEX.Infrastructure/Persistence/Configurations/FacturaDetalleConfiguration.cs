using System;
using System.Collections.Generic;
using System.Text;


using ExamenCAEX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamenCAEX.Infrastructure.Persistence.Configurations;

public sealed class FacturaDetalleConfiguration
    : IEntityTypeConfiguration<FacturaDetalle>
{
    public void Configure(
        EntityTypeBuilder<FacturaDetalle> builder)
    {
        // Tabla existente
        builder.ToTable("FacturaDetalles", "examen");

        // Clave primaria
        builder.HasKey(d => d.FacturaDetalleId);

        builder.Property(d => d.FacturaDetalleId)
            .ValueGeneratedOnAdd();

        // Factura relacionada
        builder.Property(d => d.FacturaId)
            .IsRequired();

        // Producto relacionado
        builder.Property(d => d.ProductoId)
            .IsRequired();

        // Cantidad
        builder.Property(d => d.Cantidad)
            .IsRequired();

        // Precio histórico
        builder.Property(d => d.PrecioUnitario)
            .IsRequired()
            .HasPrecision(18, 2);

        // Subtotal calculado en SQL Server
        builder.Property(d => d.Subtotal)
            .HasComputedColumnSql(
                "CONVERT(DECIMAL(18,2), " +
                "[Cantidad] * [PrecioUnitario])",
                stored: true);

        // Relación con Producto
        builder.HasOne<Producto>()
            .WithMany()
            .HasForeignKey(d => d.ProductoId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName(
                "FK_FacturaDetalles_Productos");

        // Índices existentes
        builder.HasIndex(d => d.FacturaId)
            .HasDatabaseName(
                "IX_FacturaDetalles_FacturaId");

        builder.HasIndex(d => d.ProductoId)
            .HasDatabaseName(
                "IX_FacturaDetalles_ProductoId");
    }
}
