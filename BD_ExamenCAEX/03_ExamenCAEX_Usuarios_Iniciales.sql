/* EXAMEN CAEX - USUARIOS DE DEMOSTRACION
   Compatible con ASP.NET Core Identity PasswordHasher V3.
   Crea usuarios faltantes y ACTUALIZA contrasena, rol y estado de los existentes.
   Ejecutar DESPUES del script 01. No compartir las credenciales en produccion.
*/
USE [ExamenCAEX];
GO
SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;
    IF EXISTS (SELECT 1 FROM examen.Usuarios WHERE NombreUsuario = N'admincaex')
    BEGIN
        UPDATE examen.Usuarios SET PasswordHash = N'AQAAAAIAAYagAAAAEKUMGdJ0XuLh8XLtduaLM2vB7XFW4wj9E6mMrot6S/BOhR6XouU+TmQGL4hviewKmQ==', Rol = N'ADMIN', Activo = 1
        WHERE NombreUsuario = N'admincaex';
    END
    ELSE
    BEGIN
        INSERT INTO examen.Usuarios (NombreUsuario, PasswordHash, Rol, Activo)
        VALUES (N'admincaex', N'AQAAAAIAAYagAAAAEKUMGdJ0XuLh8XLtduaLM2vB7XFW4wj9E6mMrot6S/BOhR6XouU+TmQGL4hviewKmQ==', N'ADMIN', 1);
    END

    IF EXISTS (SELECT 1 FROM examen.Usuarios WHERE NombreUsuario = N'usuario_prueba')
    BEGIN
        UPDATE examen.Usuarios SET PasswordHash = N'AQAAAAIAAYagAAAAEO2fDrIrHWhXIxta8Lt7waWpBkg3widPS7bHy4y/quQqE5xAMkCD0HqwxSOd1nVyuw==', Rol = N'USUARIO', Activo = 1
        WHERE NombreUsuario = N'usuario_prueba';
    END
    ELSE
    BEGIN
        INSERT INTO examen.Usuarios (NombreUsuario, PasswordHash, Rol, Activo)
        VALUES (N'usuario_prueba', N'AQAAAAIAAYagAAAAEO2fDrIrHWhXIxta8Lt7waWpBkg3widPS7bHy4y/quQqE5xAMkCD0HqwxSOd1nVyuw==', N'USUARIO', 1);
    END

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO
SELECT NombreUsuario, Rol, Activo FROM examen.Usuarios WHERE NombreUsuario IN (N'admincaex', N'usuario_prueba') ORDER BY NombreUsuario;
GO
