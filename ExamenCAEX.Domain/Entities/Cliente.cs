using System;
using System.Collections.Generic;
using System.Text;


namespace ExamenCAEX.Domain.Entities;

public class Cliente
{
    public int ClienteId { get; private set; }

    public string Nombre { get; private set; } = string.Empty;

    public string NIT { get; private set; } = string.Empty;

    public string? Email { get; private set; }

    public string? Telefono { get; private set; }

    public string? Direccion { get; private set; }

    public bool Activo { get; private set; } = true;

    public DateTime FechaRegistro { get; private set; }


    private Cliente() { }

    public Cliente(
        string nombre,
        string nit,
        string? email,
        string? telefono,
        string? direccion)
    {
        ValidarDatos(nombre, nit);

        Nombre = nombre.Trim();
        NIT = nit.Trim();
        Email = email?.Trim();
        Telefono = telefono?.Trim();
        Direccion = direccion?.Trim();
        Activo = true;
        FechaRegistro = DateTime.UtcNow;
    }

    public void Actualizar(
        string nombre,
        string nit,
        string? email,
        string? telefono,
        string? direccion)
    {
        ValidarDatos(nombre, nit);

        Nombre = nombre.Trim();
        NIT = nit.Trim();
        Email = email?.Trim();
        Telefono = telefono?.Trim();
        Direccion = direccion?.Trim();
    }

    public void Desactivar()
    {
        Activo = false;
    }

    public void Activar()
    {
        Activo = true;
    }

    private static void ValidarDatos(
        string nombre,
        string nit)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException(
                "El nombre es obligatorio.");

        if (nombre.Trim().Length > 150)
            throw new ArgumentException(
                "El nombre no puede superar 150 caracteres.");

        if (string.IsNullOrWhiteSpace(nit))
            throw new ArgumentException(
                "El NIT es obligatorio.");

        if (nit.Trim().Length > 20)
            throw new ArgumentException(
                "El NIT no puede superar 20 caracteres.");
    }
}
