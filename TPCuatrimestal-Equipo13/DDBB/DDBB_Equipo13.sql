use master
go
CREATE DATABASE BBDD_Equipo13
GO
USE BBDD_Equipo13
GO
--TABLA PARA TIPOS DE VEHICULOS
CREATE TABLE TIPOS_VEHICULOS(
    IDTIPO INT PRIMARY KEY IDENTITY(1,1),
    TIPO VARCHAR(20) NOT NULL,
    CANT_ASIENTOS INT NULL
)
GO
--RESTRICCIONES TIPOS DE VEHICULOS
ALTER TABLE TIPOS_VEHICULOS
ADD CHECK(CANT_ASIENTOS >= 4)
GO

--TABLA PARA VEHICULOS
CREATE TABLE VEHICULOS(
    IDVEHICULO INT PRIMARY KEY IDENTITY(1,1),
    IDTIPO INT NOT NULL,
    MODELO INT NOT NULL DEFAULT(2022),
    PATENTE VARCHAR(7) NOT NULL,
    ESTADO BIT NULL DEFAULT (1)
)
GO
--RESTRICCIONES VEHICULOS
ALTER TABLE VEHICULOS
ADD CONSTRAINT FK_VEHICULO_TIPO FOREIGN KEY (IDTIPO) REFERENCES TIPOS_VEHICULOS (IDTIPO)
GO
ALTER TABLE VEHICULOS
ADD CHECK(MODELO <= YEAR(GETDATE()))
GO

--TABLA PARA DOMICILIO
CREATE TABLE DOMICILIO (
    IDDOMICILIO BIGINT PRIMARY KEY IDENTITY(1,1),
    DIRECCION VARCHAR(100) NOT NULL,
    LOCALIDAD VARCHAR(80) NOT NULL,
    PROVINCIA VARCHAR(25) NOT NULL,
    DESCRIPCION VARCHAR(200) NULL,
)
GO  

-- TABLA PARA PERSONAS
CREATE TABLE PERSONA (
    IDPERSONA INT PRIMARY KEY IDENTITY(1,1),
    NOMBRES VARCHAR(50) NOT NULL,
    APELLIDOS VARCHAR(50) NOT NULL,
    DNI VARCHAR(10) NULL,
    FECHANACIMIENTO DATE NULL,
    DOMICILIO BIGINT FOREIGN KEY REFERENCES DOMICILIO(IDDOMICILIO) NULL,
    TELEFONO VARCHAR(15) NOT NULL DEFAULT 1588888,
    EMAIL VARCHAR(30) NOT NULL,
    NACIONALIDAD VARCHAR(40) NULL
)
GO

--TABLA PARA ZONAS
CREATE TABLE ZONAS(
    IDZONA INT PRIMARY KEY IDENTITY(1,1),
    NOMBREZONA VARCHAR(15) NOT NULL
)
GO

-- TABLA PARA CHOFERES
CREATE TABLE CHOFER (
    IDCHOFER INT PRIMARY KEY IDENTITY (1,1),
    IDPERSONA INT FOREIGN KEY REFERENCES PERSONA (IDPERSONA) NOT NULL,
    IDZONA INT FOREIGN KEY REFERENCES ZONAS (IDZONA) NOT NULL,
    IDVEHICULO INT FOREIGN KEY REFERENCES VEHICULOS (IDVEHICULO) NULL,
    ESTADO BIT NOT NULL DEFAULT 1
)
GO

-- TABLA PARA CLIENTES
CREATE TABLE CLIENTE (
	IDCLIENTE INT PRIMARY KEY IDENTITY (1, 1),
	IDPERSONA INT NOT NULL FOREIGN KEY REFERENCES PERSONA (IDPERSONA),
    IDZONA INT NOT NULL FOREIGN KEY REFERENCES ZONAS (IDZONA) DEFAULT 2,
    ESTADO BIT NOT NULL DEFAULT (1)
)
GO

-- TABLA PARA VIAJES
CREATE TABLE VIAJES (
    IDVIAJE BIGINT PRIMARY KEY IDENTITY(1,1),
    IDCHOFER INT NULL FOREIGN KEY REFERENCES CHOFER (IDCHOFER),
    IDCLIENTE INT NULL FOREIGN KEY REFERENCES CLIENTE (IDCLIENTE),
    TIPOVIAJE VARCHAR(15) NULL,
    IMPORTE MONEY NOT NULL,
    IDDOMORIGEN BIGINT NOT NULL FOREIGN KEY REFERENCES DOMICILIO (IDDOMICILIO),
    IDDOMDESTINO1 BIGINT NOT NULL FOREIGN KEY REFERENCES DOMICILIO (IDDOMICILIO),
    IDDOMDESTINO2 BIGINT NULL FOREIGN KEY REFERENCES DOMICILIO (IDDOMICILIO),    
    IDDOMDESTINO3 BIGINT NULL FOREIGN KEY REFERENCES DOMICILIO (IDDOMICILIO),    
    ESTADO VARCHAR(15) NOT NULL DEFAULT 'ASIGNADO',
    FECHAHORAVIAJE DATETIME NOT NULL DEFAULT GETDATE(),
    PAGADO BIT NOT NULL DEFAULT 0,
    MEDIODEPAGO VARCHAR(25) NOT NULL
)
GO

--TABLA PARA USUARIOS
CREATE TABLE USUARIO (
    EMAIL VARCHAR(50) PRIMARY KEY,
    CONTRASENIA VARCHAR(15) NOT NULL,
    ESADMIN BIT DEFAULT 1 NOT NULL,
    IDCHOFER INT NULL FOREIGN KEY REFERENCES CHOFER(IDCHOFER)
)
-- RESTRICCIONES VIAJES
ALTER TABLE VIAJES
ADD CHECK (IMPORTE >= 0)
GO

-- STORED PROCEDURE
GO
CREATE PROCEDURE SP_BAJALOGICACLIENTE (@IDCLIENTE INT)
AS BEGIN

    UPDATE CLIENTE SET ESTADO = 0 WHERE @IDCLIENTE = IDCLIENTE
END


--STORE PROCEDURE RESUMENES DE VIAJES
GO
CREATE PROCEDURE SP_RESUMENES_VIAJES (
    @IDPERSONA INT,
    @IDENTIFICADOR BIT,
    @FECHAIN DATETIME,
    @FECHAFIN DATETIME
)
AS
BEGIN
    IF @IDENTIFICADOR = 1 BEGIN
        DECLARE @CHOFER INT
        SELECT @CHOFER = IDCHOFER FROM CHOFER WHERE IDPERSONA = @IDPERSONA
        SELECT 
            V.TIPOVIAJE,
            V.IMPORTE,
            V.FECHAHORAVIAJE
        FROM VIAJES AS V 
        WHERE IDCHOFER = @CHOFER
        AND V.FECHAHORAVIAJE >= @FECHAIN
        AND V.FECHAHORAVIAJE <= @FECHAFIN
        AND UPPER(ESTADO) <> 'CANCELADO'
        GROUP BY V.IDCHOFER, V.TIPOVIAJE, V.IMPORTE, V.FECHAHORAVIAJE
        ORDER BY V.FECHAHORAVIAJE ASC

    END
    ELSE BEGIN
        DECLARE @CLIENTE INT
        SELECT @CLIENTE = IDCLIENTE FROM CLIENTE WHERE IDPERSONA = @IDPERSONA

        SELECT 
            V.TIPOVIAJE,
            V.IMPORTE,
            V.FECHAHORAVIAJE
        FROM VIAJES AS V 
        WHERE IDCLIENTE = @CLIENTE
        AND V.FECHAHORAVIAJE >= @FECHAIN
        AND V.FECHAHORAVIAJE <= @FECHAFIN
        AND UPPER(ESTADO) <> 'CANCELADO'
        GROUP BY V.IDCLIENTE, V.TIPOVIAJE, V.IMPORTE, V.FECHAHORAVIAJE
        ORDER BY V.FECHAHORAVIAJE ASC
    END

END

--STORED PROCEDURE CREADO PARA FILTRO
GO
CREATE PROCEDURE SP_VIAJES_X_DIA(
    @CAMPO VARCHAR(25),
    @BUSQUEDA VARCHAR(25),
    @FECHA DATE
)
AS 
BEGIN
    IF @CAMPO <> 'CHOFER' BEGIN
        SELECT V.IDVIAJE, V.IDCLIENTE, V.IDCHOFER, V.TIPOVIAJE, V.IMPORTE, V.IDDOMORIGEN, V.IDDOMDESTINO1,
        V.IDDOMDESTINO2, V.IDDOMDESTINO3, V.ESTADO, V.FECHAHORAVIAJE, V.PAGADO, V.MEDIODEPAGO
        FROM VIAJES AS V 
        WHERE UPPER(V.TIPOVIAJE) LIKE '%'+ UPPER(@BUSQUEDA) + '%'
        OR UPPER(V.ESTADO) LIKE '%'+ UPPER(@BUSQUEDA) + '%'
        GROUP BY V.IDVIAJE, V.IDCLIENTE, V.IDCHOFER,
        V.TIPOVIAJE, V.IMPORTE, V.IDDOMORIGEN,
        V.IDDOMDESTINO1, V.IDDOMDESTINO2, V.IDDOMDESTINO3,
        V.ESTADO, V.FECHAHORAVIAJE, V.PAGADO, V.MEDIODEPAGO
        HAVING CONVERT(DATE, V.FECHAHORAVIAJE) = @FECHA
        ORDER BY V.FECHAHORAVIAJE ASC
    END
    ELSE BEGIN
        SELECT V.IDVIAJE, V.IDCLIENTE, V.IDCHOFER, V.TIPOVIAJE, V.IMPORTE, V.IDDOMORIGEN, V.IDDOMDESTINO1,
        V.IDDOMDESTINO2, V.IDDOMDESTINO3, V.ESTADO, V.FECHAHORAVIAJE, V.PAGADO, V.MEDIODEPAGO
        FROM VIAJES AS V
        INNER JOIN CHOFER AS C ON V.IDCHOFER = C.IDCHOFER
        INNER JOIN PERSONA AS P ON C.IDPERSONA = P.IDPERSONA
        INNER JOIN ZONAS AS Z ON C.IDZONA = Z.IDZONA
        WHERE UPPER(P.NOMBRES) LIKE '%' + UPPER(@BUSQUEDA) + '%'
        OR UPPER(P.APELLIDOS) LIKE '%' + UPPER(@BUSQUEDA) + '%'
        OR UPPER(Z.NOMBREZONA) LIKE '%' + UPPER(@BUSQUEDA) + '%'
        GROUP BY V.IDVIAJE, V.IDCLIENTE, V.IDCHOFER, V.TIPOVIAJE, V.IMPORTE, 
        V.IDDOMORIGEN, V.IDDOMDESTINO1, V.IDDOMDESTINO2, V.IDDOMDESTINO3,
        V.ESTADO, V.FECHAHORAVIAJE, V.PAGADO, V.MEDIODEPAGO 
        HAVING CONVERT(DATE, V.FECHAHORAVIAJE) =  @FECHA 
        ORDER BY V.FECHAHORAVIAJE ASC
    END

END
