# Sistema de Control Clínico Veterinario v2

Aplicación de escritorio en C# (Windows Forms, .NET 8) con base de datos SQLite.

## Módulos
- **Clientes**: CRUD de dueños de mascotas
- **Mascotas**: CRUD, vinculadas a un cliente
- **Veterinarios**: CRUD
- **Consultas**: CRUD, vinculadas a una mascota y a un veterinario

## Estructura
- `Modelos/`: clases de datos
- `Datos/`: conexión SQLite, creación de tablas y DAOs con el CRUD
- `Formularios/`: formularios de Windows Forms

## Ejecución
1. Abrir `VeterinariaApp.csproj` en Visual Studio 2022 (con la carga de trabajo de escritorio .NET).
2. Restaurar paquetes NuGet (`Microsoft.Data.Sqlite`) y ejecutar con F5.
3. La base de datos `veterinaria.db` y sus tablas se crean automáticamente al iniciar.
