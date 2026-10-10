using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using ExamenCAEX.Application.Interfaces;
using ExamenCAEX.Web.Models.Facturas;
using ExamenCAEX.Application.DTOs.Facturas;
using Microsoft.AspNetCore.Authorization;

namespace ExamenCAEX.Web.Controllers;

[Authorize]

public class FacturasController : Controller
{
    private readonly IFacturaService _facturaService;

    private readonly IClienteService _clienteService;
    private readonly IProductoService _productoService;


    public FacturasController(
        IFacturaService facturaService,
        IClienteService clienteService,
        IProductoService productoService)
    {
        _facturaService = facturaService;
        _clienteService = clienteService;
        _productoService = productoService;
    }


  
    [HttpGet]
    public async Task<IActionResult> Index(
        CancellationToken cancellationToken)
    {
        var facturas = await _facturaService.ObtenerTodosAsync(
            cancellationToken);

        return View(facturas);
    }

  
    [HttpGet]
    public async Task<IActionResult> Detalle(
        int id,
        CancellationToken cancellationToken)
    {
        var factura = await _facturaService.ObtenerPorIdAsync(
            id,
            cancellationToken);

        if (factura is null)
        {
            return NotFound();
        }

        return View(factura);
    }



    [HttpGet]
    public async Task<IActionResult> Crear(
        CancellationToken cancellationToken)
    {
        var clientes = await _clienteService.ObtenerTodosAsync(
            cancellationToken);

        var productos = await _productoService.ObtenerTodosAsync(
            cancellationToken);

        var modelo = new CrearFacturaViewModel
        {
            Detalles = new List<CrearFacturaDetalleViewModel>
        {
            new CrearFacturaDetalleViewModel()
        },

            Clientes = clientes
                .Where(c => c.Activo)
                .Select(c => new SelectListItem
                {
                    Value = c.ClienteId.ToString(),
                    Text = c.Nombre
                })
                .ToList(),

            Productos = productos
                .Where(p => p.Activo)
                .Select(p => new SelectListItem
                {
                    Value = p.ProductoId.ToString(),
                    Text = $"{p.Codigo} - {p.Nombre} - Q {p.Precio:N2}"
                })
                .ToList()
        };

        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(
        CrearFacturaViewModel modelo,
        CancellationToken cancellationToken)
    {
  
        if (modelo.Detalles is null ||
            modelo.Detalles.Count == 0)
        {
            ModelState.AddModelError(
                "",
                "Debe agregar al menos un producto.");
        }

     
        if (modelo.Detalles is not null &&
            modelo.Detalles
                .Where(d => d.ProductoId > 0)
                .GroupBy(d => d.ProductoId)
                .Any(g => g.Count() > 1))
        {
            ModelState.AddModelError(
                "",
                "No puede agregar el mismo producto dos veces.");
        }

        
        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync(
                modelo,
                cancellationToken);

            return View(modelo);
        }

        try
        {
           
            var dto = new CrearFacturaDto(
                modelo.ClienteId,
                modelo.Detalles.Select(d =>
                    new CrearFacturaDetalleDto(
                        d.ProductoId,
                        d.Cantidad
                    )
                ).ToList()
            );

            
            var facturaId = await _facturaService.CrearAsync(
                dto,
                cancellationToken);

            TempData["MensajeExito"] =
                "Factura emitida correctamente.";

           
            return RedirectToAction(
                nameof(Detalle),
                new { id = facturaId });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
        }
        catch (OverflowException)
        {
            ModelState.AddModelError(
                "",
                "El total de la factura excede el importe permitido.");
        }

        await CargarOpcionesAsync(
            modelo,
            cancellationToken);

        return View(modelo);
    }


    private async Task CargarOpcionesAsync(
        CrearFacturaViewModel modelo,
        CancellationToken cancellationToken)
    {
        var clientes = await _clienteService.ObtenerTodosAsync(
            cancellationToken);

        var productos = await _productoService.ObtenerTodosAsync(
            cancellationToken);

        modelo.Clientes = clientes
            .Where(c => c.Activo)
            .Select(c => new SelectListItem
            {
                Value = c.ClienteId.ToString(),
                Text = c.Nombre
            })
            .ToList();

        modelo.Productos = productos
            .Where(p => p.Activo)
            .Select(p => new SelectListItem
            {
                Value = p.ProductoId.ToString(),
                Text = $"{p.Codigo} - {p.Nombre} - Q {p.Precio:N2}"
            })
            .ToList();
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Anular(
        int id,
        string motivo,
        CancellationToken cancellationToken)
    {
        try
        {
            await _facturaService.AnularAsync(
                id,
                motivo,
                cancellationToken);

            TempData["MensajeExito"] =
                "Factura anulada correctamente.";
        }
        catch (ArgumentException ex)
        {
            TempData["MensajeError"] = ex.Message;
        }
        catch (InvalidOperationException ex)
        {
            TempData["MensajeError"] = ex.Message;
        }

        return RedirectToAction(
            nameof(Detalle),
            new { id });
    }

}
