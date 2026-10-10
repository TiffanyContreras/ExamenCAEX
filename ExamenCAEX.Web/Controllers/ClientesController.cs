
using ExamenCAEX.Application.DTOs.Clientes;
using ExamenCAEX.Application.Interfaces;
using ExamenCAEX.Web.Models.Clientes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ExamenCAEX.Web.Controllers;

[Authorize]
public sealed class ClientesController : Controller
{
    private readonly IClienteService _clienteService;

    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

   
    [HttpGet]
    public async Task<IActionResult> Index(
        CancellationToken cancellationToken)
    {
        var clientes = await _clienteService
            .ObtenerTodosAsync(cancellationToken);

        return View(clientes);
    }

   
    [HttpGet]
    public IActionResult Crear()
    {
        return View(new CrearClienteViewModel());
    }

   
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(
        CrearClienteViewModel modelo,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        try
        {
            var dto = new CrearClienteDto(
                modelo.Nombre,
                modelo.NIT,
                modelo.Email,
                modelo.Telefono,
                modelo.Direccion
            );

            await _clienteService.CrearAsync(
                dto,
                cancellationToken);

            TempData["MensajeExito"] =
                "Cliente registrado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            return View(modelo);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            return View(modelo);
        }
    }


    [HttpGet]
    public async Task<IActionResult> Editar(
        int id,
        CancellationToken cancellationToken)
    {
        var cliente = await _clienteService
            .ObtenerPorIdAsync(id, cancellationToken);

        if (cliente is null)
            return NotFound();

        var modelo = new EditarClienteViewModel
        {
            ClienteId = cliente.ClienteId,
            Nombre = cliente.Nombre,
            NIT = cliente.NIT,
            Email = cliente.Email,
            Telefono = cliente.Telefono,
            Direccion = cliente.Direccion
        };

        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(
        int id,
        EditarClienteViewModel modelo,
        CancellationToken cancellationToken)
    {
        if (id != modelo.ClienteId)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(modelo);

        try
        {
            var dto = new CrearClienteDto(
                modelo.Nombre,
                modelo.NIT,
                modelo.Email,
                modelo.Telefono,
                modelo.Direccion
            );

            await _clienteService.ActualizarAsync(
                id, dto, cancellationToken);

            TempData["MensajeExito"] =
                "Cliente actualizado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(modelo);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(modelo);
        }
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Desactivar(
        int id,
        CancellationToken cancellationToken)
    {
        var cliente = await _clienteService
            .ObtenerPorIdAsync(id, cancellationToken);

        if (cliente is null)
        {
            return NotFound();
        }

        if (!cliente.Activo)
        {
            TempData["MensajeError"] =
                "El cliente ya se encuentra inactivo.";

            return RedirectToAction(nameof(Index));
        }

        try
        {
            await _clienteService.DesactivarAsync(
                id,
                cancellationToken);

            TempData["MensajeExito"] =
                "Cliente desactivado correctamente.";
        }
        catch (InvalidOperationException ex)
        {
            TempData["MensajeError"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }


}

