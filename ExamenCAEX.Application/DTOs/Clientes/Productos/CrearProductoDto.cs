
namespace ExamenCAEX.Application.DTOs.Productos;

public sealed record CrearProductoDto(
    string Codigo,
    string Nombre,
    string? Descripcion,
    decimal Precio
);
