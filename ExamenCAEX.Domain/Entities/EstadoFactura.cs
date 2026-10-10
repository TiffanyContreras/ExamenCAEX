
namespace ExamenCAEX.Domain.Entities;

public class EstadoFactura
{
    public int EstadoFacturaId { get; private set; }

    public string Codigo { get; private set; } = string.Empty;

    public string Descripcion { get; private set; } = string.Empty;

  
    private EstadoFactura() { }

    public EstadoFactura(string codigo, string descripcion)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            throw new ArgumentException(
                "El código del estado es obligatorio.",
                nameof(codigo));

        if (codigo.Trim().Length > 20)
            throw new ArgumentException(
                "El código no puede superar 20 caracteres.",
                nameof(codigo));

        if (string.IsNullOrWhiteSpace(descripcion))
            throw new ArgumentException(
                "La descripción es obligatoria.",
                nameof(descripcion));

        if (descripcion.Trim().Length > 100)
            throw new ArgumentException(
                "La descripción no puede superar 100 caracteres.",
                nameof(descripcion));

        Codigo = codigo.Trim();
        Descripcion = descripcion.Trim();
    }
}
