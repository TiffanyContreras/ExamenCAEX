
namespace ExamenCAEX.Application.Interfaces;

public interface IUsuarioService
{
    Task RegistrarUsuarioAsync(
        string nombreUsuario,
        string password,
        CancellationToken cancellationToken = default);
}
