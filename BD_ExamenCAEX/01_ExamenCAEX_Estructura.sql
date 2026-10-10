/*
  EXAMEN CAEX - SCRIPT DE ESTRUCTURA Y CATALOGOS
  Motor: Microsoft SQL Server
  Esquema: examen
  Contenido: base de datos, tablas, claves, restricciones, indices,
             secuencia de facturas y catalogo de estados.

  EJECUCION: Abrir en SSMS y ejecutar sobre una instancia con permisos
  para crear bases de datos y objetos. No elimina datos existentes.
  IMPORTANTE: Ejecutar 03_ExamenCAEX_Usuarios_Iniciales.sql para
  crear o actualizar las credenciales de demostracion.
  No incluye facturas ni registros de prueba.
*/

-- 01. BASE DE DATOS
USE [master];
GO
IF DB_ID(N'ExamenCAEX') IS NULL
BEGIN
    CREATE DATABASE [ExamenCAEX];
END;
GO
USE [ExamenCAEX];
GO

-- 02. ESQUEMA
IF SCHEMA_ID(N'examen') IS NULL
    EXEC(N'CREATE SCHEMA [examen]');
GO

-- 03. CATALOGO DE CLIENTES
IF OBJECT_ID(N'examen.Clientes', N'U') IS NULL
BEGIN
    CREATE TABLE examen.Clientes
    (
        ClienteId INT IDENTITY(1,1) NOT NULL,
        Nombre NVARCHAR(150) NOT NULL,
        NIT VARCHAR(20) NOT NULL,
        Email NVARCHAR(150) NULL,
        Telefono VARCHAR(20) NULL,
        Direccion NVARCHAR(250) NULL,
        Activo BIT NOT NULL CONSTRAINT DF_Clientes_Activo DEFAULT (1),
        FechaRegistro DATETIME2 NOT NULL CONSTRAINT DF_Clientes_FechaRegistro DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT PK_Clientes PRIMARY KEY (ClienteId),
        CONSTRAINT UQ_Clientes_NIT UNIQUE (NIT),
        CONSTRAINT CK_Clientes_Nombre CHECK (LEN(TRIM(Nombre)) > 0),
        CONSTRAINT CK_Clientes_NIT CHECK (LEN(TRIM(NIT)) > 0)
    );
END;
GO

-- 04. CATALOGO DE PRODUCTOS
IF OBJECT_ID(N'examen.Productos', N'U') IS NULL
BEGIN
    CREATE TABLE examen.Productos
    (
        ProductoId INT IDENTITY(1,1) NOT NULL,
        Codigo VARCHAR(30) NOT NULL,
        Nombre NVARCHAR(150) NOT NULL,
        Descripcion NVARCHAR(250) NULL,
        Precio DECIMAL(18,2) NOT NULL,
        Activo BIT NOT NULL CONSTRAINT DF_Productos_Activo DEFAULT (1),
        FechaRegistro DATETIME2 NOT NULL CONSTRAINT DF_Productos_FechaRegistro DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT PK_Productos PRIMARY KEY (ProductoId),
        CONSTRAINT UQ_Productos_Codigo UNIQUE (Codigo),
        CONSTRAINT CK_Productos_Precio CHECK (Precio >= 0),
        CONSTRAINT CK_Productos_Nombre CHECK (LEN(TRIM(Nombre)) > 0),
        CONSTRAINT CK_Productos_Codigo CHECK (LEN(TRIM(Codigo)) > 0)
    );
END;
GO

-- 05. ESTADOS DE FACTURA
IF OBJECT_ID(N'examen.EstadosFactura', N'U') IS NULL
BEGIN
    CREATE TABLE examen.EstadosFactura
    (
        EstadoFacturaId INT IDENTITY(1,1) NOT NULL,
        Codigo VARCHAR(20) NOT NULL,
        Descripcion NVARCHAR(100) NOT NULL,
        CONSTRAINT PK_EstadosFactura PRIMARY KEY (EstadoFacturaId),
        CONSTRAINT UQ_EstadosFactura_Codigo UNIQUE (Codigo),
        CONSTRAINT CK_EstadosFactura_Codigo CHECK (LEN(TRIM(Codigo)) > 0),
        CONSTRAINT CK_EstadosFactura_Descripcion CHECK (LEN(TRIM(Descripcion)) > 0)
    );
END;
GO
IF NOT EXISTS (SELECT 1 FROM examen.EstadosFactura WHERE Codigo = 'EMITIDA')
    INSERT INTO examen.EstadosFactura (Codigo, Descripcion) VALUES ('EMITIDA', N'Factura emitida');
IF NOT EXISTS (SELECT 1 FROM examen.EstadosFactura WHERE Codigo = 'ANULADA')
    INSERT INTO examen.EstadosFactura (Codigo, Descripcion) VALUES ('ANULADA', N'Factura anulada');
GO

-- 06. FACTURAS: ENCABEZADO (MAESTRO)
IF OBJECT_ID(N'examen.Facturas', N'U') IS NULL
BEGIN
    CREATE TABLE examen.Facturas
    (
        FacturaId INT IDENTITY(1,1) NOT NULL,
        NumeroFactura VARCHAR(30) NOT NULL,
        ClienteId INT NOT NULL,
        EstadoFacturaId INT NOT NULL,
        FechaEmision DATETIME2 NOT NULL CONSTRAINT DF_Facturas_FechaEmision DEFAULT (SYSUTCDATETIME()),
        Total DECIMAL(18,2) NOT NULL,
        FechaAnulacion DATETIME2 NULL,
        MotivoAnulacion NVARCHAR(250) NULL,
        CONSTRAINT PK_Facturas PRIMARY KEY (FacturaId),
        CONSTRAINT UQ_Facturas_Numero UNIQUE (NumeroFactura),
        CONSTRAINT FK_Facturas_Clientes FOREIGN KEY (ClienteId) REFERENCES examen.Clientes(ClienteId),
        CONSTRAINT FK_Facturas_Estados FOREIGN KEY (EstadoFacturaId) REFERENCES examen.EstadosFactura(EstadoFacturaId),
        CONSTRAINT CK_Facturas_Total CHECK (Total >= 0),
        CONSTRAINT CK_Facturas_Numero CHECK (LEN(TRIM(NumeroFactura)) > 0),
        CONSTRAINT CK_Facturas_Anulacion CHECK
        (
            (FechaAnulacion IS NULL AND MotivoAnulacion IS NULL)
            OR
            (FechaAnulacion IS NOT NULL AND MotivoAnulacion IS NOT NULL
             AND LEN(TRIM(MotivoAnulacion)) > 0)
        )
    );
END;
GO

-- 07. FACTURAS: DETALLE
IF OBJECT_ID(N'examen.FacturaDetalles', N'U') IS NULL
BEGIN
    CREATE TABLE examen.FacturaDetalles
    (
        FacturaDetalleId INT IDENTITY(1,1) NOT NULL,
        FacturaId INT NOT NULL,
        ProductoId INT NOT NULL,
        Cantidad INT NOT NULL,
        PrecioUnitario DECIMAL(18,2) NOT NULL,
        Subtotal AS (CONVERT(DECIMAL(18,2), Cantidad * PrecioUnitario)) PERSISTED,
        CONSTRAINT PK_FacturaDetalles PRIMARY KEY (FacturaDetalleId),
        CONSTRAINT FK_FacturaDetalles_Facturas FOREIGN KEY (FacturaId) REFERENCES examen.Facturas(FacturaId),
        CONSTRAINT FK_FacturaDetalles_Productos FOREIGN KEY (ProductoId) REFERENCES examen.Productos(ProductoId),
        CONSTRAINT CK_FacturaDetalles_Cantidad CHECK (Cantidad > 0),
        CONSTRAINT CK_FacturaDetalles_Precio CHECK (PrecioUnitario >= 0)
    );
END;
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'examen.FacturaDetalles') AND name = N'IX_FacturaDetalles_FacturaId')
    CREATE INDEX IX_FacturaDetalles_FacturaId ON examen.FacturaDetalles(FacturaId);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'examen.FacturaDetalles') AND name = N'IX_FacturaDetalles_ProductoId')
    CREATE INDEX IX_FacturaDetalles_ProductoId ON examen.FacturaDetalles(ProductoId);
GO

-- 08. USUARIOS Y ROLES
IF OBJECT_ID(N'examen.Usuarios', N'U') IS NULL
BEGIN
    CREATE TABLE examen.Usuarios
    (
        UsuarioId INT IDENTITY(1,1) NOT NULL,
        NombreUsuario NVARCHAR(100) NOT NULL,
        PasswordHash NVARCHAR(500) NOT NULL,
        Rol NVARCHAR(20) NOT NULL CONSTRAINT DF_Usuarios_Rol DEFAULT ('USUARIO'),
        Activo BIT NOT NULL CONSTRAINT DF_Usuarios_Activo DEFAULT (1),
        FechaRegistro DATETIME2 NOT NULL CONSTRAINT DF_Usuarios_FechaRegistro DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT PK_Usuarios PRIMARY KEY (UsuarioId),
        CONSTRAINT UQ_Usuarios_NombreUsuario UNIQUE (NombreUsuario),
        CONSTRAINT CK_Usuarios_Rol CHECK (Rol IN ('ADMIN', 'USUARIO'))
    );
END;
GO

-- 09. SECUENCIA DE NUMERACION DE FACTURAS
-- Para una instalacion nueva comienza en 1. Si la base ya tiene facturas,
-- no se debe reiniciar ni retroceder la secuencia existente.
IF OBJECT_ID(N'examen.SecuenciaFacturas', N'SO') IS NULL
BEGIN
    EXEC(N'CREATE SEQUENCE examen.SecuenciaFacturas AS BIGINT START WITH 1 INCREMENT BY 1');
END;
GO

PRINT N'Estructura de ExamenCAEX preparada. Verifique las tablas y la secuencia.';
GO
