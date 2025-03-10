# API CRUD .NET 8

Este proyecto es una API CRUD desarrollada en .NET 8. Proporciona una estructura básica para crear, leer, actualizar y eliminar recursos.

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)

## Estructura del Proyecto

- `src/Bootstrapper/Api`: Contiene la API principal.
- `src/Modules/Memberships`: Módulo de membresías.
- `src/Modules/Memberships/Users`: Submódulo de usuarios.
- `src/Shared`: Código compartido entre módulos.
- `tests/UnitTests`: Pruebas unitarias.

## Configuración

1. Clona el repositorio:
    ```sh
    git clone https://github.com/angelsepulveda/CrudBackend.git
    ```

2. Restaura los paquetes NuGet:
    ```sh
    dotnet restore
    ```

## Construcción y Ejecución

### Localmente

Para construir y ejecutar la API localmente:

Para construir la imagen de la database:

1. Construye la imagen de Docker de sqlserver y levantarlo (opcional: Si tiene un servidor sql server puede utilizar ese o si no levanta la imagen docker):
    ```sh
    docker-compose up -d
    ```
2. Ejecuta el script de sql ubicado en scripts/scriptpruebatecnica.sql en el servidor de sqlserver:

3. Construye el proyecto:
    ```sh
    dotnet build
    ```

4. Ejecuta la API:
    ```sh
    dotnet run --project src/Bootstrapper/Api/Api.csproj
    ```

## Uso

La API expone los siguientes endpoints:

- `POST /api/users`: Crea un nuevo usuario.
- `GET /api/users`: Obtener listado de usuarios.
- `PUT /api/users`: Actualizar un usuario.
- `PUT /api/users/{id}`: eliminiar un usuario.
- `GET /api/users/export`: exporta el listado de usuarios a excel.
- `POST /api/auth/login-google`: inicio de sesión para loguarse en el sistema.
- `POST /api/auth/logout`: cierra sesión de un usuario autentificado.

## Pruebas

Para ejecutar las pruebas unitarias:

```sh
dotnet test