
using ExamenCAEX.Application.Interfaces;
using ExamenCAEX.Web.Models.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamenCAEX.Web.Controllers;

[Authorize(Roles = "ADMIN")]
public class UsuariosController : Controller
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

   
    [HttpGet]
    public IActionResult Crear()
    {
        return View(new CrearUsuarioViewModel());
    }

   
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(
        CrearUsuarioViewModel modelo,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        try
        {
            await _usuarioService.RegistrarUsuarioAsync(
                modelo.NombreUsuario,
                modelo.Password,
                cancellationToken);

            TempData["MensajeExito"] =
                "Usuario registrado correctamente.";

            return RedirectToAction(nameof(Crear));
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
}
