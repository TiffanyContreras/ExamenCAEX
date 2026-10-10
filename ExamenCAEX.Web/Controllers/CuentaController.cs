
using System.Security.Claims;
using ExamenCAEX.Application.Interfaces;
using ExamenCAEX.Web.Models.Cuenta;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamenCAEX.Web.Controllers;

public class CuentaController : Controller
{
    private readonly IAuthService _authService;

    public CuentaController(IAuthService authService)
    {
        _authService = authService;
    }

   
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Clientes");
        }

        return View(new LoginViewModel());
    }

    
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        LoginViewModel modelo,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

   
        var usuario = await _authService.ValidarCredencialesAsync(
            modelo.NombreUsuario,
            modelo.Password,
            cancellationToken);

        if (usuario is null)
        {
            ModelState.AddModelError(
                "",
                "Usuario o contraseña incorrectos.");

            return View(modelo);
        }

       
        var claims = new List<Claim>
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                usuario.UsuarioId.ToString()),

            new Claim(
                ClaimTypes.Name,
                usuario.NombreUsuario),

            new Claim(
                ClaimTypes.Role,
                usuario.Rol)
        };

      
        var identidad = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identidad);

     
        var propiedades = new AuthenticationProperties
        {
            IsPersistent = modelo.Recordarme,

            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(
                modelo.Recordarme ? 120 : 30)
        };

      
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            propiedades);

        return RedirectToAction("Index", "Clientes");
    }

    
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Salir()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction(nameof(Login));
    }

   
    [HttpGet]
    public IActionResult AccesoDenegado()
    {
        return View();
    }
}
