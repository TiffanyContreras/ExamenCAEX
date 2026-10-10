
using ExamenCAEX.Domain.Entities;
using ExamenCAEX.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExamenCAEX.Web.Inicializacion;

public static class UsuarioInicializador
{
    public static async Task InicializarAsync(
        IServiceProvider servicios)
    {

        using var scope = servicios.CreateScope();

        var contexto = scope.ServiceProvider
            .GetRequiredService<ExamenDbContext>();

        var passwordHasher = scope.ServiceProvider
            .GetRequiredService<IPasswordHasher<Usuario>>();

        if (await contexto.Usuarios.AnyAsync())
        {
            return;
        }

        var nombreUsuario = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(nombreUsuario))
        {
            throw new InvalidOperationException(
                "Debe ingresar un nombre de usuario.");
        }

        Console.Write("Contraseña inicial: ");
        var password = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(password) ||
            password.Length < 12)
        {
            throw new InvalidOperationException(
                "La contraseña debe tener al menos 12 caracteres.");
        }

        var usuario = new Usuario(nombreUsuario, "TEMPORAL");

        var hash = passwordHasher.HashPassword(
            usuario,
            password);


        var usuarioInicial = new Usuario(
            nombreUsuario,
            hash);

        contexto.Usuarios.Add(usuarioInicial);

        await contexto.SaveChangesAsync();

        Console.WriteLine(
            "Usuario inicial creado correctamente.");
    }
}
