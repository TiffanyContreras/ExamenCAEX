using System;
using System.Collections.Generic;
using System.Text;


namespace ExamenCAEX.Application.DTOs.Reportes;

public sealed record ResumenFacturasDto(
    int TotalFacturas,
    int FacturasEmitidas,
    int FacturasAnuladas,
    decimal MontoFacturasEmitidas
);

