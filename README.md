# ExamenCAEX

Aplicación web de gestión de clientes, productos y facturación, desarrollada con **ASP.NET Core MVC (.NET 10)** y **SQL Server**, organizada en capas.

## Estructura de la solución

- `ExamenCAEX.Domain`: entidades del dominio.
- `ExamenCAEX.Application`: contratos, DTO y servicios de aplicación.
- `ExamenCAEX.Infrastructure`: acceso a datos, Entity Framework Core e inyección de dependencias.
- `ExamenCAEX.Web`: controladores MVC, vistas, autenticación y configuración web.
- `Database`: scripts SQL de instalación, usuarios iniciales y verificaciones.

## Funcionalidades

- Inicio y cierre de sesión con autenticación por cookies.
- Roles `ADMIN` y `USUARIO`; gestión de usuarios restringida a administradores.
- Gestión de clientes y productos con desactivación lógica.
- Emisión y anulación de facturas con detalle de productos.
- Consulta de facturas y reportes por rango de fechas, con exportación CSV.

## Requisitos

- SDK de .NET 10.
- SQL Server y SQL Server Management Studio (SSMS) o herramienta equivalente.
- Visual Studio con soporte para ASP.NET Core (opcional; también se puede usar la CLI de .NET).

## Preparación de la base de datos

En SQL Server, ejecutar **en este orden** los archivos de `Database`:

1. `01_ExamenCAEX_Estructura.sql`: crea la base `ExamenCAEX`, el esquema `examen` y los objetos necesarios si no existen.
2. `03_ExamenCAEX_Usuarios_Iniciales.sql`: crea o actualiza dos cuentas de demostración (administrador y usuario normal).
3. `02_ExamenCAEX_Verificaciones.sql`: consulta la estructura y comprueba la integridad.

**Atención:** los scripts están dirigidos a la base `ExamenCAEX`. El script de usuarios actualiza credenciales si las cuentas ya existen. No ejecutarlo sobre un entorno productivo sin revisión previa.

## Configuración y ejecución

1. Abrir la solución `ExamenCAEX.sln` o `ExamenCAEX.slnx` en Visual Studio.
2. Configurar la cadena de conexión en `ExamenCAEX.Web/appsettings.json` o, preferiblemente, mediante configuración local/secretos de usuario. Indicar el servidor SQL Server propio y la base `ExamenCAEX`.
3. No publicar contraseñas de SQL Server, claves ni secretos en Git.
4. Restaurar dependencias, compilar y ejecutar `ExamenCAEX.Web` como proyecto de inicio.
5. Acceder a la URL HTTPS mostrada por la aplicación y probar el inicio de sesión.

## Cuentas de demostración

- Administrador: `admincaex` (rol `ADMIN`).
- Usuario: `usuario_prueba` (rol `USUARIO`).


## Validación

- Ejecutar el script `02_ExamenCAEX_Verificaciones.sql` y revisar sus resultados.
- Verificar acceso al Login sin sesión, acceso a los módulos con sesión y bloqueo de la gestión de usuarios para el rol `USUARIO`.
- Comprobar creación/anulación de facturas y reportes.


