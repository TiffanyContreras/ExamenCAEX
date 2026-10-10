using System;
using System.Collections.Generic;
using System.Text;


namespace ExamenCAEX.Application.DTOs.Clientes;

public sealed record CrearClienteDto(
    string Nombre,
    string NIT,
    string? Email,
    string? Telefono,
    string? Direccion
);
