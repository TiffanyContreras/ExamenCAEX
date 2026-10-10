using System;
using System.Collections.Generic;
using System.Text;


using ExamenCAEX.Application.DTOs.Reportes;

namespace ExamenCAEX.Application.Interfaces;

public interface IReporteService
{
    Task<ResumenFacturasDto> ObtenerResumenFacturasAsync(
        DateTime? fechaInicio = null,
        DateTime? fechaFin = null,
        CancellationToken cancellationToken = default);
}
