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

--CARGA DE DATOS TIPOS DE VEHICULOS
INSERT INTO TIPOS_VEHICULOS (TIPO, CANT_ASIENTOS) VALUES ('Camioneta', 4);
INSERT INTO TIPOS_VEHICULOS (TIPO, CANT_ASIENTOS) VALUES ('Sedan', 4);
INSERT INTO TIPOS_VEHICULOS (TIPO, CANT_ASIENTOS) VALUES ('Van', 11);
INSERT INTO TIPOS_VEHICULOS (TIPO, CANT_ASIENTOS) VALUES ('Mini Van', 6);
INSERT INTO TIPOS_VEHICULOS (TIPO, CANT_ASIENTOS) VALUES ('Ejecutivo', 4);
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
ADD CHECK(MODELO <= GETDATE())
GO

--CARGA DE DATOS VEHICULOS
INSERT INTO VEHICULOS (IDTIPO, MODELO, PATENTE, ESTADO) VALUES (1, 2020, 'ABC123', 1);
INSERT INTO VEHICULOS (IDTIPO, MODELO, PATENTE, ESTADO) VALUES (2, 2022, 'XYZ789', 1);
INSERT INTO VEHICULOS (IDTIPO, MODELO, PATENTE, ESTADO) VALUES (3, 2021, 'LMN456', 0);
INSERT INTO VEHICULOS (IDTIPO, MODELO, PATENTE, ESTADO) VALUES (4, 2022, 'PQR987', 1);
INSERT INTO VEHICULOS (IDTIPO, MODELO, PATENTE, ESTADO) VALUES (5, 2023, 'AD456FE', 1);
INSERT INTO VEHICULOS (IDTIPO, MODELO, PATENTE, ESTADO) VALUES (2, 2021, 'GHI789', 0);
INSERT INTO VEHICULOS (IDTIPO, MODELO, PATENTE, ESTADO) VALUES (3, 2020, 'JKL123', 1);
INSERT INTO VEHICULOS (IDTIPO, MODELO, PATENTE, ESTADO) VALUES (4, 2022, 'MNO789', 1);
INSERT INTO VEHICULOS (IDTIPO, MODELO, PATENTE, ESTADO) VALUES (5, 2023, 'AE456FR', 0);
INSERT INTO VEHICULOS (IDTIPO, MODELO, PATENTE, ESTADO) VALUES (2, 2021, 'UVW123', 1);
INSERT INTO VEHICULOS (IDTIPO, MODELO, PATENTE, ESTADO) VALUES (5, 2021, 'AB343GR', 1);

/*--TABLA PARA DOMICILIOS
CREATE TABLE DOMICILIOS(
    IDDOMICILIO BIGINT,
    DIRECCION VARCHAR(300),
    DESCRIPCION VARCHAR(200) NULL
)
GO
--RESTRICCIONES DOMICILIOS
ALTER TABLE DOMICILIOS
ADD CONSTRAINT PK_DOMICILIOS PRIMARY KEY (IDDOMICILIO)
GO*/

ALTER TABLE VEHICULOS
DROP CONSTRAINT CK_VEHICULOSMODEL_29572725

ALTER TABLE VEHICULOS
DROP column MODELO

ALTER TABLE VEHICULOS
ADD MODELO INT NOT NULL DEFAULT(2022)

DROP DATABASE BBDD_Equipo13