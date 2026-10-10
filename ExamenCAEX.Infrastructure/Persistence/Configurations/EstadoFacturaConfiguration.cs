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
        
        builder.ToTable("EstadosFactura", "examen");

     
        builder.HasKey(e => e.EstadoFacturaId);

        builder.Property(e => e.EstadoFacturaId)
            .ValueGeneratedOnAdd();

       
        builder.Property(e => e.Codigo)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(20);

       
        builder.Property(e => e.Descripcion)
            .IsRequired()
            .HasMaxLength(100);

       
        builder.HasAlternateKey(e => e.Codigo)
            .HasName("UQ_EstadosFactura_Codigo");
    }
}
