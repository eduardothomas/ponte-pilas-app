# Ponte Pilas App

Tareas con fecha, calendario y recordatorios que insisten: push, correo y, si es urgente, WhatsApp. Este repo contiene el cliente (web, Android, iOS) y los contratos compartidos. El backend está en un repo privado.

## Inicio rápido

**Requisitos:** .NET SDK 10

```bash
git clone https://github.com/eduardothomas/ponte-pilas-app.git
cd ponte-pilas-app
dotnet restore PontePilas.slnx
dotnet test PontePilas.slnx
```

## Accesos

| Qué | Enlace | Se actualiza |
|---|---|---|
| Producción (web) | No aplica todavía | — |
| QA | No aplica | — |
| Previews de PR | No aplica | — |
| GitHub Actions | https://github.com/eduardothomas/ponte-pilas-app/actions | En cada push y PR |
| Repositorio | https://github.com/eduardothomas/ponte-pilas-app | — |
| Backend (privado) | https://github.com/eduardothomas/ponte-pilas-api | — |
| Panel de hosting | No aplica todavía (se usará Azure) | — |

## Contenido del proyecto

```
src/PontePilas.Shared/        Contratos (DTOs y enums) compartidos con la API
tests/PontePilas.Shared.Tests Pruebas xUnit
```

- **Shared:** modelos de tarea, prioridad, repetición y canales de aviso.

## Pruebas y build

```bash
dotnet build PontePilas.slnx
dotnet test PontePilas.slnx
```

## Despliegue
Pendiente. Se usará Azure (Static Web Apps para la web).

## Flujo de cambios
Ramas de trabajo y PR según el Estándar de Desarrollo de Software de Eduardo (v1.2). Detalles para agentes en `AGENTS.md`.

## Volver a una versión estable
Revertir el commit del PR en `main` (`git revert <sha>`) y abrir un PR con la reversión.

## Licencia
[Business Source License 1.1](LICENSE). Puedes ver, modificar y usar el código para uso personal, educativo o de evaluación. No está permitido ofrecerlo como producto o servicio comercial. El 18 de octubre de 2030 pasa a Apache 2.0.
