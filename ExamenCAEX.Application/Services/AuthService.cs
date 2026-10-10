
using ExamenCAEX.Application.Interfaces;
using ExamenCAEX.Application.Repositories;
using ExamenCAEX.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using ExamenCAEX.Application.DTOs.Usuarios;

namespace ExamenCAEX.Application.Services;

public sealed class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        IPasswordHasher<Usuario> passwordHasher)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UsuarioAutenticadoDto?> ValidarCredencialesAsync(
        string nombreUsuario,
        string password,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario) ||
            string.IsNullOrWhiteSpace(password))
        {
            return null;
        }

        var usuario = await _usuarioRepository.ObtenerPorNombreAsync(
            nombreUsuario.Trim(),
            cancellationToken);

        if (usuario is null || !usuario.Activo)
        {
            return null;
        }

        var resultado = _passwordHasher.VerifyHashedPassword(
            usuario,
            usuario.PasswordHash,
            password);

        if (resultado == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return new UsuarioAutenticadoDto(
    usuario.UsuarioId,
    usuario.NombreUsuario,
    usuario.Rol);
    }
}
