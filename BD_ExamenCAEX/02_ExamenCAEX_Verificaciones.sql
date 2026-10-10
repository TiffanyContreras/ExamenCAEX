/* EXAMEN CAEX - CONSULTAS DE VERIFICACION (SOLO LECTURA)
   Ejecutar despues del script ExamenCAEX_Estructura.sql.
   No inserta, modifica ni elimina registros.
*/
USE [ExamenCAEX];
GO
-- 01. Tablas del esquema
SELECT s.name AS Esquema, t.name AS Tabla
FROM sys.tables t INNER JOIN sys.schemas s ON s.schema_id=t.schema_id
WHERE s.name=N'examen' ORDER BY t.name;

-- 02. Relaciones y confianza de claves foraneas
SELECT fk.name AS Relacion, OBJECT_NAME(fk.parent_object_id) AS TablaOrigen,
       OBJECT_NAME(fk.referenced_object_id) AS TablaDestino,
       fk.is_disabled AS Deshabilitada, fk.is_not_trusted AS NoConfiable
FROM sys.foreign_keys fk
WHERE SCHEMA_NAME(OBJECTPROPERTY(fk.parent_object_id, 'SchemaId'))=N'examen'
ORDER BY TablaOrigen, TablaDestino;

-- 03. Estados iniciales
SELECT EstadoFacturaId, Codigo, Descripcion FROM examen.EstadosFactura ORDER BY EstadoFacturaId;

-- 04. Secuencia
SELECT name, current_value, is_cached FROM sys.sequences
WHERE name=N'SecuenciaFacturas' AND schema_id=SCHEMA_ID(N'examen');

-- 05. Usuarios y roles (sin exponer hashes)
SELECT UsuarioId, NombreUsuario, Rol, Activo, FechaRegistro FROM examen.Usuarios ORDER BY UsuarioId;

-- 06. Facturas y detalles
SELECT f.FacturaId, f.NumeroFactura, e.Codigo AS Estado, f.Total,
       COUNT(d.FacturaDetalleId) AS CantidadDetalles
FROM examen.Facturas f
INNER JOIN examen.EstadosFactura e ON e.EstadoFacturaId=f.EstadoFacturaId
LEFT JOIN examen.FacturaDetalles d ON d.FacturaId=f.FacturaId
GROUP BY f.FacturaId, f.NumeroFactura, e.Codigo, f.Total
ORDER BY f.FacturaId;

-- 07. Diferencia entre totales (se espera cero en todas las facturas con detalle)
SELECT f.NumeroFactura, f.Total AS TotalFactura,
       SUM(d.Subtotal) AS TotalDetalles,
       f.Total-SUM(d.Subtotal) AS Diferencia
FROM examen.Facturas f
INNER JOIN examen.FacturaDetalles d ON d.FacturaId=f.FacturaId
GROUP BY f.NumeroFactura, f.Total;

-- 08. Detalles sin factura (se esperan cero filas)
SELECT d.FacturaDetalleId, d.FacturaId, d.ProductoId
FROM examen.FacturaDetalles d
LEFT JOIN examen.Facturas f ON f.FacturaId=d.FacturaId
WHERE f.FacturaId IS NULL;

-- 09. Facturas duplicadas (se esperan cero filas)
SELECT NumeroFactura, COUNT(*) AS VecesRepetido
FROM examen.Facturas GROUP BY NumeroFactura HAVING COUNT(*)>1;

-- 10. Indices unicos de catalogos y facturas
SELECT t.name AS Tabla, i.name AS Restriccion, i.is_unique AS EsUnica
FROM sys.indexes i
INNER JOIN sys.tables t ON t.object_id=i.object_id
INNER JOIN sys.schemas s ON s.schema_id=t.schema_id
WHERE s.name=N'examen' AND t.name IN (N'Clientes',N'Productos',N'Facturas',N'Usuarios') AND i.is_unique=1
ORDER BY t.name, i.name;
GO
