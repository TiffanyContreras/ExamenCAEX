
namespace ExamenCAEX.Domain.Entities;

public class Producto
{
    public int ProductoId { get; private set; }

    public string Codigo { get; private set; } = string.Empty;

    public string Nombre { get; private set; } = string.Empty;

    public string? Descripcion { get; private set; }

    public decimal Precio { get; private set; }

    public bool Activo { get; private set; } = true;

    public DateTime FechaRegistro { get; private set; }

    // Constructor para Entity Framework Core
    private Producto() { }

    public Producto(
        string codigo,
        string nombre,
        string? descripcion,
        decimal precio)
    {
        ValidarDatos(codigo, nombre, descripcion, precio);

        Codigo = codigo.Trim();
        Nombre = nombre.Trim();
        Descripcion = descripcion?.Trim();
        Precio = precio;
        Activo = true;
        FechaRegistro = DateTime.UtcNow;
        ValidarPrecio(precio);
    }

    public void Actualizar(
        string codigo,
        string nombre,
        string? descripcion,
        decimal precio)
    {
        ValidarDatos(codigo, nombre, descripcion, precio);

        Codigo = codigo.Trim();
        Nombre = nombre.Trim();
        Descripcion = descripcion?.Trim();
        Precio = precio;
        ValidarPrecio(precio);
    }

    public void Activar()
    {
        Activo = true;
    }

    public void Desactivar()
    {
        Activo = false;
    }

    private static void ValidarDatos(
        string codigo,
        string nombre,
        string? descripcion,
        decimal precio)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            throw new ArgumentException(
                "El código del producto es obligatorio.");

        if (codigo.Trim().Length > 30)
            throw new ArgumentException(
                "El código no puede superar 30 caracteres.");

        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException(
                "El nombre del producto es obligatorio.");

        if (nombre.Trim().Length > 150)
            throw new ArgumentException(
                "El nombre no puede superar 150 caracteres.");

        if (descripcion?.Length > 250)
            throw new ArgumentException(
                "La descripción no puede superar 250 caracteres.");

        if (precio < 0 || precio > 9999999999999999.99m)
            throw new ArgumentOutOfRangeException(
                nameof(precio),
                "El precio debe estar dentro del rango permitido.");
    }

    private static void ValidarPrecio(decimal precio)
    {
        if (precio < 0)
        {
            throw new ArgumentException(
                "El precio no puede ser negativo.");
        }

        if (precio > 9999999999999999.99m)
        {
            throw new ArgumentException(
                "El precio excede el valor permitido.");
        }

        if (decimal.Round(precio, 2) != precio)
        {
            throw new ArgumentException(
                "El precio debe tener como máximo dos decimales.");
        }
    }

}
