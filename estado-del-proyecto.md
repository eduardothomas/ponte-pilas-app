# Estado del proyecto

**Proyecto:** Ponte Pilas App (cliente)  
**Repositorio:** https://github.com/eduardothomas/ponte-pilas-app (público) · backend: https://github.com/eduardothomas/ponte-pilas-api (privado)  
**Estándar adoptado:** software-development-standard v1.2  
**Plataforma:** GitHub

## Estado actual
Repositorio inicializado con contratos compartidos, pruebas y CI.

## Funcionalidades terminadas
- [x] Contratos base (TareaDto, GuardarTareaRequest, enums)

## En desarrollo
- [ ] `feature/mvp-cliente`: lógica de agenda y calendario, UI Blazor, web y app MAUI

## Decisiones importantes
- 2026-10-18 Nombre: Ponte Pilas.
- 2026-10-18 Stack: .NET 10, Blazor Hybrid + .NET MAUI (iOS/Android) y Blazor WebAssembly (web) con una librería de componentes compartida.
- 2026-10-18 Cliente en repo público con licencia BSL 1.1; backend en repo privado. El backend consume `PontePilas.Shared` como submódulo Git (el repo es público, no requiere credenciales en CI).
- 2026-10-18 Hosting: Azure (Static Web Apps para la web, App Service para la API).

## Pruebas ejecutadas
- 2026-10-18 `dotnet build` de `PontePilas.Shared` en local: OK. Las pruebas xUnit corren en GitHub Actions (NuGet bloqueado en la sesión de Claude).

## Riesgos o bloqueos
- Repo público: el código se puede ver y copiar; la licencia limita el uso comercial pero no lo impide técnicamente.

## Pendientes y siguiente paso
- [ ] Fusionar el PR del MVP del cliente.
