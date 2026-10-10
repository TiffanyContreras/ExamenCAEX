
using ExamenCAEX.Application.DTOs.Usuarios;

namespace ExamenCAEX.Application.Interfaces;

public interface IAuthService
{
    Task<UsuarioAutenticadoDto?> ValidarCredencialesAsync(
        string nombreUsuario,
        string password,
        CancellationToken cancellationToken = default);
}
