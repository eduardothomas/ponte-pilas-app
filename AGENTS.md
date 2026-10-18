# AGENTS.md — Ponte Pilas App

Este proyecto aplica el Estándar de Desarrollo de Software de Eduardo **v1.2** (`eduardothomas/software-development-standard`). Plataforma: **GitHub**.

## Qué es
App de tareas y recordatorios con fecha, calendario y avisos escalonados (push → correo → WhatsApp). Este repo es **público** (licencia BSL 1.1) y contiene solo el cliente: web, app móvil y contratos compartidos. El backend vive en el repo **privado** `eduardothomas/ponte-pilas-api`.

## Arquitectura
- `src/PontePilas.Shared`: DTOs, enums, repeticiones, agenda, calendario y validación, sin dependencias. La API privada lo consume como submódulo Git. **Cualquier cambio aquí puede romper la API.**
- `src/PontePilas.UI`: Razor Class Library con las pantallas (Hoy, Calendario, Editor), el cliente HTTP (`ITareasCliente`) y los estilos (`wwwroot/css/pilas.css`). Todo lo visual va aquí para que web y app sean iguales.
- `src/PontePilas.Web`: host Blazor WebAssembly. URL de la API en `wwwroot/appsettings.json` (en producción la escribe el workflow desde la variable `API_URL`).
- `src/PontePilas.App`: host .NET MAUI Blazor Hybrid. URL de la API en `ConfiguracionApi.cs`. iOS solo compila en macOS.
- `tests/PontePilas.Shared.Tests`: pruebas xUnit de `Shared`.
- .NET 10 (ver `global.json`).

## Comandos
- Pruebas: `dotnet test tests/PontePilas.Shared.Tests`
- Web: `dotnet run --project src/PontePilas.Web` (http://localhost:5173)
- Android: `dotnet build src/PontePilas.App -p:Plataforma=android -f net10.0-android`
- iOS (macOS): `dotnet build src/PontePilas.App -p:Plataforma=ios -f net10.0-ios`
- No compilar la solución completa en Linux: la app MAUI necesita su workload. Usar los proyectos por separado.

## Reglas
- Nada de secretos, llaves ni reglas de negocio sensibles en este repo: es público y el código del cliente se puede descompilar. La lógica de avisos y las integraciones (WhatsApp, correo, base de datos) van en la API privada.
- Mantener `Shared` sin paquetes NuGet externos.

## Flujo de Git
- Rama base: `main`. Ramas `feature/*`, `fix/*`, `docs/*`, `hotfix/*` en español.
- Todo cambio entra por PR con pruebas y build ejecutados de verdad.
- No fusionar a producción ni desplegar sin autorización de Eduardo.
- Si cambia `Shared`, abrir también el PR en `ponte-pilas-api` que actualiza el submódulo.
