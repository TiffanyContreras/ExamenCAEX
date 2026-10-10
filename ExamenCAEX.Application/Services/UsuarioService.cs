
using ExamenCAEX.Application.Interfaces;
using ExamenCAEX.Application.Repositories;
using ExamenCAEX.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ExamenCAEX.Application.Services;

public sealed class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public UsuarioService(
        IUsuarioRepository usuarioRepository,
        IPasswordHasher<Usuario> passwordHasher)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task RegistrarUsuarioAsync(
        string nombreUsuario,
        string password,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario))
        {
            throw new ArgumentException(
                "El nombre de usuario es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(password) ||
            password.Length < 12)
        {
            throw new ArgumentException(
                "La contraseña debe tener al menos 12 caracteres.");
        }

        nombreUsuario = nombreUsuario.Trim();

        var existente = await _usuarioRepository.ObtenerPorNombreAsync(
            nombreUsuario,
            cancellationToken);

        if (existente is not null)
        {
            throw new InvalidOperationException(
                "El nombre de usuario ya está registrado.");
        }


        var usuarioTemporal = new Usuario(
            nombreUsuario,
            "TEMPORAL",
            "USUARIO");

        var passwordHash = _passwordHasher.HashPassword(
            usuarioTemporal,
            password);

      
        var usuario = new Usuario(
            nombreUsuario,
            passwordHash,
            "USUARIO");

        await _usuarioRepository.AgregarAsync(
            usuario,
            cancellationToken);

        await _usuarioRepository.GuardarCambiosAsync(
            cancellationToken);
    }
}
