using ExamenCAEX.Application.Interfaces;
using ExamenCAEX.Application.Repositories;
using ExamenCAEX.Application.Services;
using ExamenCAEX.Infrastructure.Persistence;
using ExamenCAEX.Infrastructure.Persistence.Repositories;
using ExamenCAEX.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using ExamenCAEX.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ExamenCAEX.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration
            .GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "No se configuró la conexión DefaultConnection.");
        }

       
        services.AddDbContext<ExamenDbContext>(options =>
            options.UseSqlServer(connectionString));

        
        services.AddScoped<IClienteRepository, ClienteRepository>();

        services.AddScoped<IProductoRepository, ProductoRepository>();

        services.AddScoped<IClienteService, ClienteService>();

        services.AddScoped<IProductoService, ProductoService>();

        services.AddScoped<IFacturaRepository, FacturaRepository>();
        services.AddScoped<IFacturaService, FacturaService>();
        services.AddScoped<IReporteService, ReporteService>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPasswordHasher<Usuario>,PasswordHasher<Usuario>>();
        services.AddScoped<IUsuarioService, UsuarioService>();


        return services;
    }
}
