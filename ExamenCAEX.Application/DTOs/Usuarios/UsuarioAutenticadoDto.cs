using System;
using System.Collections.Generic;
using System.Text;


namespace ExamenCAEX.Application.DTOs.Usuarios;

public sealed record UsuarioAutenticadoDto(
    int UsuarioId,
    string NombreUsuario,
    string Rol);

