using System;
using System.Collections.Generic;
using System.Text;

using ExamenCAEX.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamenCAEX.Infrastructure.Persistence;

public class ExamenDbContext : DbContext
{
    public ExamenDbContext(
        DbContextOptions<ExamenDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cliente> Clientes => Set<Cliente>();

    public DbSet<Producto> Productos => Set<Producto>();

    public DbSet<Factura> Facturas => Set<Factura>();

    public DbSet<FacturaDetalle> FacturaDetalles =>
        Set<FacturaDetalle>();

    public DbSet<EstadoFactura> EstadosFactura =>
        Set<EstadoFactura>();


    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("examen");

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ExamenDbContext).Assembly);
    }


}
