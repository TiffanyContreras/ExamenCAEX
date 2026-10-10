using System;
using System.Collections.Generic;
using System.Text;

using ExamenCAEX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamenCAEX.Infrastructure.Persistence.Configurations;

public sealed class EstadoFacturaConfiguration
    : IEntityTypeConfiguration<EstadoFactura>
{
    public void Configure(
        EntityTypeBuilder<EstadoFactura> builder)
    {
        // Tabla existente en SQL Server
        builder.ToTable("EstadosFactura", "examen");

        // Clave primaria
        builder.HasKey(e => e.EstadoFacturaId);

        builder.Property(e => e.EstadoFacturaId)
            .ValueGeneratedOnAdd();

        // Código obligatorio
        builder.Property(e => e.Codigo)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(20);

        // Descripción obligatoria
        builder.Property(e => e.Descripcion)
            .IsRequired()
            .HasMaxLength(100);

        // Restricción de unicidad existente
        builder.HasAlternateKey(e => e.Codigo)
            .HasName("UQ_EstadosFactura_Codigo");
    }
}
