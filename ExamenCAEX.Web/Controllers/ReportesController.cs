using ExamenCAEX.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using ClosedXML.Excel;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
namespace ExamenCAEX.Web.Controllers;

[Authorize]
public class ReportesController : Controller
{
    private readonly IReporteService _reporteService;
    private readonly IFacturaService _facturaService;

    public ReportesController(
        IReporteService reporteService,
        IFacturaService facturaService)
    {
        _reporteService = reporteService;
        _facturaService = facturaService;
    }



    [HttpGet]
    public async Task<IActionResult> Index(
        DateTime? fechaInicio,
        DateTime? fechaFin,
        CancellationToken cancellationToken)
    {

        ViewBag.FechaInicio = fechaInicio?.ToString("yyyy-MM-dd");
        ViewBag.FechaFin = fechaFin?.ToString("yyyy-MM-dd");

      
        if (fechaInicio.HasValue &&
            fechaFin.HasValue &&
            fechaInicio.Value.Date > fechaFin.Value.Date)
        {
            ModelState.AddModelError(
                "",
                "La fecha inicial no puede ser mayor que la fecha final.");
        }

        if (!ModelState.IsValid)
        {
            return View(
                new ExamenCAEX.Application.DTOs.Reportes.ResumenFacturasDto(
                    0, 0, 0, 0m));
        }

       
        var zonaGuatemala =
            TimeZoneInfo.FindSystemTimeZoneById(
                "Central America Standard Time");

        DateTime? inicioUtc = null;
        DateTime? finExclusivoUtc = null;

        if (fechaInicio.HasValue)
        {
            var inicioLocal = DateTime.SpecifyKind(
                fechaInicio.Value.Date,
                DateTimeKind.Unspecified);

            inicioUtc = TimeZoneInfo.ConvertTimeToUtc(
                inicioLocal,
                zonaGuatemala);
        }

        if (fechaFin.HasValue)
        {
            var diaSiguienteLocal = DateTime.SpecifyKind(
                fechaFin.Value.Date.AddDays(1),
                DateTimeKind.Unspecified);

            finExclusivoUtc = TimeZoneInfo.ConvertTimeToUtc(
                diaSiguienteLocal,
                zonaGuatemala);
        }

        var resumen = await _reporteService
            .ObtenerResumenFacturasAsync(
                inicioUtc,
                finExclusivoUtc,
                cancellationToken);

        return View(resumen);
    }



    [HttpGet]
    public async Task<IActionResult> ExportarCsv(
        DateTime? fechaInicio,
        DateTime? fechaFin,
        CancellationToken cancellationToken)
    {
       
        if (fechaInicio.HasValue &&
            fechaFin.HasValue &&
            fechaInicio.Value.Date > fechaFin.Value.Date)
        {
            return BadRequest(
                "La fecha inicial no puede ser mayor que la fecha final.");
        }

       
        var zonaGuatemala =
            TimeZoneInfo.FindSystemTimeZoneById(
                "Central America Standard Time");

        DateTime? inicioUtc = null;
        DateTime? finExclusivoUtc = null;

        
        if (fechaInicio.HasValue)
        {
            var inicioLocal = DateTime.SpecifyKind(
                fechaInicio.Value.Date,
                DateTimeKind.Unspecified);

            inicioUtc = TimeZoneInfo.ConvertTimeToUtc(
                inicioLocal,
                zonaGuatemala);
        }


        if (fechaFin.HasValue)
        {
            var diaSiguienteLocal = DateTime.SpecifyKind(
                fechaFin.Value.Date.AddDays(1),
                DateTimeKind.Unspecified);

            finExclusivoUtc = TimeZoneInfo.ConvertTimeToUtc(
                diaSiguienteLocal,
                zonaGuatemala);
        }

       
        var facturas = await _facturaService.ObtenerTodosAsync(
            cancellationToken);


        var filtradas = facturas.Where(f =>
            (!inicioUtc.HasValue ||
             f.FechaEmision >= inicioUtc.Value) &&
            (!finExclusivoUtc.HasValue ||
             f.FechaEmision < finExclusivoUtc.Value)
        ).ToList();

     
        var csv = new StringBuilder();

        csv.AppendLine(
            "NumeroFactura,Cliente,FechaEmisionUTC,Estado,Total");

        foreach (var factura in filtradas)
        {
            static string Escapar(string? valor)
            {
               
                valor ??= "";

                if (valor.Length > 0 &&
                    "=+-@\t\r\n".Contains(valor[0]))
                {
                    valor = "'" + valor;
                }

                return "\"" + valor.Replace("\"", "\"\"") + "\"";
            }

            csv.AppendLine(string.Join(",",
                Escapar(factura.NumeroFactura),
                Escapar(factura.NombreCliente),
                Escapar(factura.FechaEmision.ToString(
                    "yyyy-MM-dd HH:mm:ss")),
                Escapar(factura.Estado),
                factura.Total.ToString(
                    System.Globalization.CultureInfo.InvariantCulture)
            ));
        }

        
        var contenido = Encoding.UTF8.GetPreamble()
            .Concat(Encoding.UTF8.GetBytes(csv.ToString()))
            .ToArray();

        return File(
            contenido,
            "text/csv; charset=utf-8",
            "ReporteFacturas.csv");
    }



    [HttpGet]
    public async Task<IActionResult> ExportarExcel(
        DateTime? fechaInicio,
        DateTime? fechaFin,
        CancellationToken cancellationToken)
    {
       
        if (fechaInicio.HasValue &&
            fechaFin.HasValue &&
            fechaInicio.Value.Date > fechaFin.Value.Date)
        {
            return BadRequest(
                "La fecha inicial no puede ser mayor que la fecha final.");
        }

      
        var zonaGuatemala =
            TimeZoneInfo.FindSystemTimeZoneById(
                "Central America Standard Time");

        DateTime? inicioUtc = null;
        DateTime? finExclusivoUtc = null;

        if (fechaInicio.HasValue)
        {
            var inicioLocal = DateTime.SpecifyKind(
                fechaInicio.Value.Date,
                DateTimeKind.Unspecified);

            inicioUtc = TimeZoneInfo.ConvertTimeToUtc(
                inicioLocal,
                zonaGuatemala);
        }

        if (fechaFin.HasValue)
        {
            var siguienteDiaLocal = DateTime.SpecifyKind(
                fechaFin.Value.Date.AddDays(1),
                DateTimeKind.Unspecified);

            finExclusivoUtc = TimeZoneInfo.ConvertTimeToUtc(
                siguienteDiaLocal,
                zonaGuatemala);
        }

      
        var facturas = await _facturaService.ObtenerTodosAsync(
            cancellationToken);

        var filtradas = facturas
            .Where(f =>
                (!inicioUtc.HasValue ||
                 f.FechaEmision >= inicioUtc.Value) &&
                (!finExclusivoUtc.HasValue ||
                 f.FechaEmision < finExclusivoUtc.Value))
            .OrderByDescending(f => f.FechaEmision)
            .ToList();

        
        using var libro = new XLWorkbook();

        var hoja = libro.Worksheets.Add("Facturas");

      
        hoja.Cell("A1").Value = "REPORTE DE FACTURACIÓN";

        hoja.Range("A1:E1").Merge();

        hoja.Cell("A1").Style.Font.Bold = true;
        hoja.Cell("A1").Style.Font.FontSize = 16;
        hoja.Cell("A1").Style.Font.FontColor = XLColor.White;
        hoja.Cell("A1").Style.Fill.BackgroundColor =
            XLColor.FromHtml("#212529");

        
        hoja.Cell("A3").Value = "Número";
        hoja.Cell("B3").Value = "Cliente";
        hoja.Cell("C3").Value = "Fecha (Guatemala)";
        hoja.Cell("D3").Value = "Estado";
        hoja.Cell("E3").Value = "Total (Q)";

        var encabezados = hoja.Range("A3:E3");

        encabezados.Style.Font.Bold = true;
        encabezados.Style.Font.FontColor = XLColor.White;
        encabezados.Style.Fill.BackgroundColor =
            XLColor.FromHtml("#0D6EFD");

        
        int fila = 4;

        foreach (var factura in filtradas)
        {
            var fechaUtc = DateTime.SpecifyKind(
                factura.FechaEmision,
                DateTimeKind.Utc);

            var fechaLocal = TimeZoneInfo.ConvertTimeFromUtc(
                fechaUtc,
                zonaGuatemala);

            hoja.Cell(fila, 1).Value = factura.NumeroFactura;
            hoja.Cell(fila, 2).Value = factura.NombreCliente;
            hoja.Cell(fila, 3).Value = fechaLocal;
            hoja.Cell(fila, 4).Value = factura.Estado;
            hoja.Cell(fila, 5).Value = factura.Total;

            hoja.Cell(fila, 3)
                .Style.DateFormat.Format = "dd/mm/yyyy hh:mm";

            hoja.Cell(fila, 5)
                .Style.NumberFormat.Format = "\"Q\" #,##0.00";

            if (factura.Estado == "ANULADA")
            {
                hoja.Cell(fila, 4)
                    .Style.Font.FontColor = XLColor.Red;
            }
            else if (factura.Estado == "EMITIDA")
            {
                hoja.Cell(fila, 4)
                    .Style.Font.FontColor = XLColor.Green;
            }

            fila++;
        }

      
        hoja.Cell(fila + 1, 4).Value = "MONTO VIGENTE:";
        hoja.Cell(fila + 1, 4).Style.Font.Bold = true;

        hoja.Cell(fila + 1, 5).Value = filtradas
            .Where(f => f.Estado == "EMITIDA")
            .Sum(f => f.Total);

        hoja.Cell(fila + 1, 5)
            .Style.NumberFormat.Format = "\"Q\" #,##0.00";

        hoja.Cell(fila + 1, 5).Style.Font.Bold = true;

       
        hoja.Columns("A:E").AdjustToContents();

        hoja.Column("A").Width =
            Math.Max(hoja.Column("A").Width, 18);

        hoja.Column("B").Width =
            Math.Max(hoja.Column("B").Width, 22);

        hoja.Column("C").Width =
            Math.Max(hoja.Column("C").Width, 23);

        hoja.SheetView.FreezeRows(3);

        if (filtradas.Count > 0)
        {
            hoja.Range(3, 1, fila - 1, 5).SetAutoFilter();
        }

      
        using var memoria = new MemoryStream();

        libro.SaveAs(memoria);

        return File(
            memoria.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "ReporteFacturas.xlsx");
    }


}

