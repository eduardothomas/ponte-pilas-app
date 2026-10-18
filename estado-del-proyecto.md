# Estado del proyecto

**Proyecto:** Ponte Pilas App (cliente)  
**Repositorio:** https://github.com/eduardothomas/ponte-pilas-app (público) · backend: https://github.com/eduardothomas/ponte-pilas-api (privado)  
**Estándar adoptado:** software-development-standard v1.2  
**Plataforma:** GitHub

## Estado actual
MVP del cliente en PR: pantallas Hoy, Calendario y Editor compartidas entre web (Blazor WebAssembly) y app (MAUI Blazor Hybrid para Android e iOS).

## Funcionalidades terminadas
- [x] Contratos base (TareaDto, GuardarTareaRequest, enums)

## En desarrollo
- [ ] `feature/mvp-cliente` (PR #1): agenda, calendario, editor, web, app MAUI, CI móvil y workflow de despliegue web

## Decisiones importantes
- 2026-10-18 Nombre: Ponte Pilas.
- 2026-10-18 Stack: .NET 10, Blazor Hybrid + .NET MAUI (iOS/Android) y Blazor WebAssembly (web) con una librería de componentes compartida.
- 2026-10-18 Cliente en repo público con licencia BSL 1.1; backend en repo privado. El backend consume `PontePilas.Shared` como submódulo Git.
- 2026-10-18 Hosting: Azure (Static Web Apps para la web, App Service para la API).
- 2026-10-18 Sin rama `develop`: los PR van directo a `main` y el CI genera el APK de prueba en cada PR (simplicidad, regla 24).
- 2026-10-18 Estilos propios (sin Bootstrap ni MudBlazor) para no agregar dependencias.

## Pruebas ejecutadas
- 2026-10-18 19 pruebas de Shared (repeticiones, agenda, calendario, validación) en local con un runner mínimo: 19 OK. En GitHub Actions: ver PR #1.

## Riesgos o bloqueos
- Repo público: el código se puede ver y copiar; la licencia limita el uso comercial pero no lo impide técnicamente.
- La URL de producción de la API en la app (`ConfiguracionApi.cs`) es provisional hasta crear el App Service.

## Pendientes y siguiente paso
- [ ] Fusionar PR #1 y luego mover el submódulo de la API a `main`.
- [ ] Inicio de sesión.
- [ ] Notificaciones push en la app (FCM / APNs).
- [ ] Crear Azure Static Web App y cargar el secreto de despliegue.
