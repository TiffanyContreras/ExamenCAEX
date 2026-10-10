
namespace ExamenCAEX.Application.DTOs.Productos;

public sealed record ProductoDto(
    int ProductoId,
    string Codigo,
    string Nombre,
    string? Descripcion,
    decimal Precio,
    bool Activo
);


