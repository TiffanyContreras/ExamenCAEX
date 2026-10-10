using System;
using System.Collections.Generic;
using System.Text;

using ExamenCAEX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamenCAEX.Infrastructure.Persistence.Configurations;

public sealed class ProductoConfiguration
    : IEntityTypeConfiguration<Producto>
{
    public void Configure(
        EntityTypeBuilder<Producto> builder)
    {
        
        builder.ToTable("Productos", "examen");

       
        builder.HasKey(p => p.ProductoId);

        builder.Property(p => p.ProductoId)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Codigo)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(30);

        builder.HasIndex(p => p.Codigo)
            .IsUnique()
            .HasDatabaseName("UQ_Productos_Codigo");

        
        builder.Property(p => p.Nombre)
            .IsRequired()
            .HasMaxLength(150);

        
        builder.Property(p => p.Descripcion)
            .HasMaxLength(250);

        
        builder.Property(p => p.Precio)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.Activo)
            .IsRequired()
            .HasDefaultValue(true);

        
        builder.Property(p => p.FechaRegistro)
            .HasColumnType("datetime2")
            .HasDefaultValueSql("SYSUTCDATETIME()");
    }
}
