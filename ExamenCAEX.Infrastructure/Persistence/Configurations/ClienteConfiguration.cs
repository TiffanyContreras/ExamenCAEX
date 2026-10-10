using System;
using System.Collections.Generic;
using System.Text;


using ExamenCAEX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamenCAEX.Infrastructure.Persistence.Configurations;

public sealed class ClienteConfiguration
    : IEntityTypeConfiguration<Cliente>
{
    public void Configure(
        EntityTypeBuilder<Cliente> builder)
    {
        
        builder.ToTable("Clientes", "examen");

       
        builder.HasKey(c => c.ClienteId);

        builder.Property(c => c.ClienteId)
            .ValueGeneratedOnAdd();

      
        builder.Property(c => c.Nombre)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.NIT)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(20);

        
        builder.Property(c => c.Email)
            .HasMaxLength(150);

        builder.Property(c => c.Telefono)
            .IsUnicode(false)
            .HasMaxLength(20);

        builder.Property(c => c.Direccion)
            .HasMaxLength(250);

        
        builder.Property(c => c.Activo)
            .IsRequired()
            .HasDefaultValue(true);

       
        builder.Property(c => c.FechaRegistro)
            .HasColumnType("datetime2")
            .HasDefaultValueSql("SYSUTCDATETIME()");

       
        builder.HasAlternateKey(c => c.NIT)
            .HasName("UQ_Clientes_NIT");
    }
}
