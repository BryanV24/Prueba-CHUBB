-- Creaciacion de la base de datos
CREATE DATABASE AseguradoraDB;
GO

USE AseguradoraDB;
GO

---------------CREACION DE TABLAS -----------------
--Creacion de la tabla de autenticacion----
CREATE TABLE Autenticacion(
AutenticacionId INT IDENTITY(1,1) PRIMARY KEY,
NombreCompleto VARCHAR(120) NOT NULL,
Correo VARCHAR(50) NOT NULL UNIQUE,
Clave VARCHAR(10) NOT  NULL,
);

GO
--Creacion de la tabla de Seguros
CREATE TABLE Seguros(
	SeguroId INT IDENTITY(1,1) PRIMARY KEY,
	NombreSeguro VARCHAR(50) NOT NULL,
	CodigoSeguro VARCHAR(10) NOT NULL UNIQUE,
	SumaAsegurada DECIMAL(12,2) NOT NULL,
	Prima DECIMAL(12,2) NOT NULL,
	Estado VARCHAR(10) NOT NULL,

	--Auditoria
	UsuarioCreacion VARCHAR(50) NOT NULL,
	FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
	UsuarioModificacion VARCHAR(50) NULL,
	FechaModificacion DATETIME NULL,

	--Validaciones
	CONSTRAINT CK_ValidacionSumaAsegurada CHECK (SumaAsegurada > 0),
	CONSTRAINT CK_ValidacionPrima CHECK (Prima > 0)
);

GO
----Creacion de la tabla de Asegurados
CREATE TABLE Asegurados(
	AseguradoId INT IDENTITY(1,1) PRIMARY KEY,
	Cedula VARCHAR(10) NOT NULL UNIQUE,
	NombreCompleto VARCHAR(150) NOT NULL,
	Telefono VARCHAR(10) NOT NULL,
	Edad INT NOT NULL,
	Estado VARCHAR(10) NOT NULL,

	--Auditoria
	UsuarioCreacion VARCHAR(50) NOT NULL,
	FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
	UsuarioModificacion VARCHAR(50) NULL,
	FechaModificacion DATETIME NULL,

	--Validaciones
	 CONSTRAINT CK_ValidacionCedula CHECK (Cedula LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
     CONSTRAINT CK_ValidacionTelefono CHECK (Telefono LIKE '09[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
	 CONSTRAINT CK_ValidacionEdad CHECK (Edad BETWEEN 0 AND 120)
);

GO
----Creacion de tabla relacion entre asegurados y seguros
CREATE TABLE AseguradosSeguros(
	AseguradosSegurosId INT IDENTITY PRIMARY KEY,
	AseguradoId INT NOT NULL,
	SeguroId INT NOT NULL,

	--Auditoria
	FechaAsignacionRelacion DATETIME DEFAULT GETDATE(),
	
	--Validaciones
	CONSTRAINT FK_AS_Asegurados FOREIGN KEY (AseguradoId) REFERENCES Asegurados(AseguradoId),
    CONSTRAINT FK_AS_Seguros FOREIGN KEY (SeguroId) REFERENCES Seguros(SeguroId),
    CONSTRAINT UQ_AseguradoSeguro UNIQUE(AseguradoId, SeguroId)
);

GO

----------------STORED PROCEDURES----------------
------Registro de usuario-------------
CREATE PROCEDURE SP_RegistroUsuario
	@NombreCompleto VARCHAR(120),
	@Correo VARCHAR(50),
	@Clave VARCHAR(10)
AS
BEGIN
	INSERT INTO dbo.Autenticacion
		(NombreCompleto, Correo, Clave)
	VALUES
	(@NombreCompleto, @Correo, @Clave);
END;

GO

-----Consulta de Usuario----------------
CREATE PROCEDURE SP_LoginUsuario
    @Correo VARCHAR(50),
    @Clave VARCHAR(10)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM dbo.Autenticacion WHERE Correo = @Correo)
    BEGIN
        SELECT 
            0 AS Success,
            'Usuario no encontrado o inactivo' AS Mensaje;
        RETURN;
    END
    IF EXISTS (
        SELECT 1 
        FROM dbo.Autenticacion 
        WHERE Correo = @Correo 
        AND Clave = @Clave
    )
    BEGIN
        SELECT 
            1 AS Success,
            'Login correcto' AS Mensaje,
            AutenticacionId,
            NombreCompleto,
            Correo
        FROM dbo.Autenticacion
        WHERE Correo = @Correo;
    END
    ELSE
    BEGIN
        SELECT 
            0 AS Success,
            'Contraseña incorrecta' AS Mensaje;
    END
END

GO

--Consultar todos los registros de los seguros
CREATE PROCEDURE SP_ObtenerSeguros
AS
BEGIN
    SELECT 
        SeguroId,
        NombreSeguro,
        CodigoSeguro,
        SumaAsegurada,
        Prima
    FROM dbo.Seguros 
	WHERE Estado = 'Activo'
    ORDER BY FechaCreacion;
END;

GO

--Consultar Asegurados por Codigo de Seguro
CREATE PROCEDURE SP_ConsultarAseguradosPorSeguro
    @CodigoSeguro VARCHAR(10)
AS
BEGIN
    SELECT
        s.CodigoSeguro,
        s.NombreSeguro,
        a.Cedula,
        a.NombreCompleto,
        a.Telefono,
        a.Edad
    FROM dbo.Seguros s
    JOIN dbo.AseguradosSeguros asignado ON s.SeguroId = asignado.SeguroId
    JOIN dbo.Asegurados a ON asignado.AseguradoId = a.AseguradoId
    WHERE s.CodigoSeguro = @CodigoSeguro;
END;

GO

----Insertar Seguros
CREATE PROCEDURE SP_InsertarSeguro
	@NombreSeguro VARCHAR(50),
	@CodigoSeguro VARCHAR(10),
	@SumaAsegurada DECIMAL(12,2),
	@Prima DECIMAL(12,2),
	@UsuarioCreacion VARCHAR(50)
AS
BEGIN
	INSERT INTO dbo.Seguros
	(NombreSeguro,CodigoSeguro, SumaAsegurada, Prima, Estado, UsuarioCreacion, FechaCreacion)

	VALUES 
	(@NombreSeguro, @CodigoSeguro, @SumaAsegurada, @Prima, 'Activo', @UsuarioCreacion, GETDATE())
END;

GO

----Actualizar Seguro
CREATE PROCEDURE SP_ActualizarSeguro
	@SeguroId INT,
	@NombreSeguro VARCHAR(50),
    @SumaAsegurada DECIMAL(12,2),
    @Prima DECIMAL(12,2),
    @UsuarioModificacion VARCHAR(50)
AS
BEGIN
	UPDATE dbo.Seguros
    SET
        NombreSeguro = @NombreSeguro,
        SumaAsegurada = @SumaAsegurada,
        Prima = @Prima,
        UsuarioModificacion = @UsuarioModificacion,
        FechaModificacion = GETDATE()
    WHERE SeguroId = @SeguroId;
END;

GO

----Eliminar Seguro
CREATE PROCEDURE SP_EliminarSeguro
    @SeguroId INT
AS
BEGIN
    UPDATE dbo.Seguros
    SET Estado = 'Inactivo'
    WHERE SeguroId = @SeguroId;
END;
GO

------ASEGURADOS---------------------------------
--Consultar todos los registros de asegurados
CREATE PROCEDURE SP_ObtenerAsegurados
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        AseguradoId,
        NombreCompleto,
        Cedula,
        Telefono,
        Edad
    FROM dbo.Asegurados
	WHERE Estado = 'Activo'
    ORDER BY FechaCreacion;
END

GO

----Consultar Seguros por la cedula del Asegurado
CREATE PROCEDURE SP_ConsultarSegurosPorCedula
    @Cedula VARCHAR(10)
AS
BEGIN
    SELECT
        a.Cedula,
        a.NombreCompleto,
        s.CodigoSeguro,
        s.NombreSeguro,
        s.SumaAsegurada,
        s.Prima
    FROM dbo.Asegurados a
    JOIN dbo.AseguradosSeguros asignado ON a.AseguradoId = asignado.AseguradoId
    JOIN dbo.Seguros s ON asignado.SeguroId = s.SeguroId
    WHERE a.Cedula = @Cedula;
END;

GO

----Insertar Asegurados
CREATE PROCEDURE SP_InsertarAsegurado
    @Cedula VARCHAR(10),
    @NombreCompleto VARCHAR(150),
    @Telefono VARCHAR(10),
    @Edad INT,
    @UsuarioCreacion VARCHAR(50)
AS
BEGIN
    INSERT INTO dbo.Asegurados
    (Cedula, NombreCompleto, Telefono, Edad, Estado, UsuarioCreacion, FechaCreacion)

    VALUES
    (@Cedula, @NombreCompleto, @Telefono, @Edad, 'Activo', @UsuarioCreacion, GETDATE());
END;

GO

----Actualizar Asegurados
CREATE PROCEDURE SP_ActualizarAsegurado
    @AseguradoId INT,
    @NombreCompleto VARCHAR(150),
    @Telefono VARCHAR(10),
    @Edad INT,
    @UsuarioModificacion VARCHAR(50)
AS
BEGIN
    UPDATE dbo.Asegurados
    SET 
        NombreCompleto = @NombreCompleto,
        Telefono = @Telefono,
        Edad = @Edad,
        UsuarioModificacion = @UsuarioModificacion,
        FechaModificacion = GETDATE()
    WHERE AseguradoId = @AseguradoId;
END;

GO

----Eliminar Asegurado
CREATE PROCEDURE SP_EliminarAsegurado
    @AseguradoId INT
AS
BEGIN
    UPDATE dbo.Asegurados
    SET Estado = 'Inactivo'
    WHERE AseguradoId = @AseguradoId;
END;

GO

----Asignacion de seguro a asegurado
CREATE PROCEDURE SP_AsignarSegurosAsegurado
    @AseguradoId INT,
    @SeguroId INT
AS
BEGIN
    ---validamos que no exista ya una relacion del seguro con el asegurado
    IF NOT EXISTS (
        SELECT 1 
        FROM dbo.AseguradosSeguros
        WHERE AseguradoId = @AseguradoId
          AND SeguroId = @SeguroId
    )
    BEGIN
        INSERT INTO dbo.AseguradosSeguros (AseguradoId, SeguroId, FechaAsignacionRelacion)
        VALUES (@AseguradoId, @SeguroId, GETDATE());
    END
    ELSE
    BEGIN
        RAISERROR('El seguro ya está asignado a este asegurado.', 16, 1);
    END
END;

GO


