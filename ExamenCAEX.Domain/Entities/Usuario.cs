
namespace ExamenCAEX.Domain.Entities;

public class Usuario
{
    public int UsuarioId { get; private set; }

    public string NombreUsuario { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public string Rol { get; private set; } = "USUARIO";

    public bool Activo { get; private set; }

    public DateTime FechaRegistro { get; private set; }

    
    private Usuario()
    {
    }

    public Usuario(
        string nombreUsuario,
        string passwordHash,
        string rol = "USUARIO")
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario))
        {
            throw new ArgumentException(
                "El nombre de usuario es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException(
                "El hash de contraseña es obligatorio.");
        }

        if (rol != "ADMIN" && rol != "USUARIO")
        {
            throw new ArgumentException(
                "El rol indicado no es válido.");
        }

        NombreUsuario = nombreUsuario.Trim();
        PasswordHash = passwordHash;
        Rol = rol;
        Activo = true;
        FechaRegistro = DateTime.UtcNow;
    }

    public void Desactivar()
    {
        Activo = false;
    }
}
