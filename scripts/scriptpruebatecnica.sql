create database pruebatecnica;

use [pruebatecnica];

CREATE TABLE users
(
	Id CHAR(36) PRIMARY KEY,
	Nombre VARCHAR(50) NOT NULL,
	Rut VARCHAR(50) NOT NULL UNIQUE,
	Correo VARCHAR(256),
	FechaNacimiento DATETIME2 NOT NULL,
	Habilitado BIT NOT NULL DEFAULT 1,
	FechaCreacion DATETIME2 NULL,
	CreadoPor NVARCHAR(MAX),
	FechaUltimaModificacion DATETIME2 NULL,
	ModificadoPor NVARCHAR(MAX) Null
);