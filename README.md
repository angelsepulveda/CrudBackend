# API CRUD .NET 8

Este proyecto es una API CRUD desarrollada en .NET 8. Proporciona una estructura básica para crear, leer, actualizar y eliminar recursos.

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)

## Estructura del Proyecto

- `src/Bootstrapper/Api`: Contiene la API principal.
- `src/Modules/Memberships`: Módulo de membresías.
- `src/Shared`: Código compartido entre módulos.
- `tests/UnitTests`: Pruebas unitarias.

## Configuración

1. Clona el repositorio:
    ```sh
    git clone https://github.com/angelsepulveda/Crud.git
    ```

2. Restaura los paquetes NuGet:
    ```sh
    dotnet restore
    ```

## Construcción y Ejecución

### Localmente

Para construir y ejecutar la API localmente:

1. Construye el proyecto:
    ```sh
    dotnet build
    ```

2. Ejecuta la API:
    ```sh
    dotnet run --project src/Bootstrapper/Api/Api.csproj
    ```

### Con Docker

Para construir la imagen de la database:

1. Construye la imagen de Docker y levantarlo:
    ```sh
    docker-compose up -d
    ```

## Uso

La API expone los siguientes endpoints:

- `POST /api/roles`: Crea un nuevo rol.
- `GET /api/roles`: Obtener listado de roles.
- `PUT /api/roles`: Actualizar un rol.
- `PUT /api/roles/{id}`: eliminiar un rol.

## Pruebas

Para ejecutar las pruebas unitarias:

```sh
dotnet test