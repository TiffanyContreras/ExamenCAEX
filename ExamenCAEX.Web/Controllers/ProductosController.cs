
using ExamenCAEX.Application.DTOs.Productos;
using ExamenCAEX.Web.Models.Productos;
using ExamenCAEX.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ExamenCAEX.Web.Controllers;

[Authorize]
public sealed class ProductosController : Controller
{
    private readonly IProductoService _productoService;

    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        CancellationToken cancellationToken)
    {
        var productos = await _productoService
            .ObtenerTodosAsync(cancellationToken);

        return View(productos);
    }


    [HttpGet]
    public IActionResult Crear()
    {
        return View(new CrearProductoViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(
        CrearProductoViewModel modelo,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        try
        {
            var dto = new CrearProductoDto(
                modelo.Codigo,
                modelo.Nombre,
                modelo.Descripcion,
                modelo.Precio!.Value
            );

            await _productoService.CrearAsync(
                dto,
                cancellationToken);

            TempData["MensajeExito"] =
                "Producto registrado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(
                string.Empty, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty, ex.Message);
        }

        return View(modelo);
    }


    [HttpGet]
    public async Task<IActionResult> Editar(
        int id,
        CancellationToken cancellationToken)
    {
        var producto = await _productoService
            .ObtenerPorIdAsync(id, cancellationToken);

        if (producto is null)
            return NotFound();

        var modelo = new EditarProductoViewModel
        {
            ProductoId = producto.ProductoId,
            Codigo = producto.Codigo,
            Nombre = producto.Nombre,
            Descripcion = producto.Descripcion,
            Precio = producto.Precio
        };

        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(
        int id,
        EditarProductoViewModel modelo,
        CancellationToken cancellationToken)
    {
        if (id != modelo.ProductoId)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(modelo);

        try
        {
            var dto = new CrearProductoDto(
                modelo.Codigo,
                modelo.Nombre,
                modelo.Descripcion,
                modelo.Precio!.Value);

            await _productoService.ActualizarAsync(
                id, dto, cancellationToken);

            TempData["MensajeExito"] =
                "Producto actualizado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
        }

        return View(modelo);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Desactivar(
        int id,
        CancellationToken cancellationToken)
    {
        var producto = await _productoService
            .ObtenerPorIdAsync(id, cancellationToken);

        if (producto is null)
        {
            return NotFound();
        }

        if (!producto.Activo)
        {
            TempData["MensajeError"] =
                "El producto ya se encuentra inactivo.";

            return RedirectToAction(nameof(Index));
        }

        try
        {
            await _productoService.DesactivarAsync(
                id, cancellationToken);

            TempData["MensajeExito"] =
                "Producto desactivado correctamente.";
        }
        catch (InvalidOperationException ex)
        {
            TempData["MensajeError"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }



}

