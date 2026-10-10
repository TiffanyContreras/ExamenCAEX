
using ExamenCAEX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamenCAEX.Infrastructure.Persistence.Configurations;

public sealed class UsuarioConfiguration
    : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios", "examen");

        builder.HasKey(u => u.UsuarioId);

        builder.Property(u => u.UsuarioId)
            .ValueGeneratedOnAdd();

        builder.Property(u => u.NombreUsuario)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(u => u.NombreUsuario)
            .IsUnique()
            .HasDatabaseName("UQ_Usuarios_NombreUsuario");

        builder.Property(u => u.Rol)
             .HasMaxLength(20)
             .IsRequired()
             .HasDefaultValue("USUARIO");

        builder.Property(u => u.PasswordHash)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(u => u.Activo)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(u => u.FechaRegistro)
            .HasColumnType("datetime2")
            .HasDefaultValueSql("SYSUTCDATETIME()");
    }
}
