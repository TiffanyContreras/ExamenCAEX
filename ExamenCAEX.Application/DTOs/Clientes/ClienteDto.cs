using System;
using System.Collections.Generic;
using System.Text;


namespace ExamenCAEX.Application.DTOs.Clientes;

public sealed record ClienteDto(
    int ClienteId,
    string Nombre,
    string NIT,
    string? Email,
    string? Telefono,
    string? Direccion,
    bool Activo
);
