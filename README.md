# Ponte Pilas App

Tareas con fecha, calendario y recordatorios que insisten: push, correo y, si es urgente, WhatsApp. Este repo tiene el cliente (web, Android e iOS) y los contratos compartidos. El backend está en un repo privado.

## Inicio rápido

**Requisitos:** .NET SDK 10. Para la app móvil: workloads de MAUI (`dotnet workload install maui-android`, y `maui-ios` en macOS con Xcode).

```bash
git clone https://github.com/eduardothomas/ponte-pilas-app.git
cd ponte-pilas-app
dotnet restore src/PontePilas.Web
dotnet run --project src/PontePilas.Web
```

La web se abre en http://localhost:5173 y espera la API en http://localhost:5080 (repo privado `ponte-pilas-api`; ver su README). La URL de la API se cambia en `src/PontePilas.Web/wwwroot/appsettings.json`.

App Android (emulador o teléfono conectado):

```bash
dotnet build src/PontePilas.App -p:Plataforma=android -f net10.0-android -t:Run
```

### En tu iPhone (desde una Mac)

Requisitos: Xcode instalado y abierto una vez, `dotnet workload install maui-ios`, el iPhone conectado por cable con el **Modo desarrollador** activado (Ajustes → Privacidad y seguridad) y tu Apple ID agregado en Xcode (Settings → Accounts). Con un Apple ID gratis la app dura 7 días en el teléfono; con Apple Developer ($99/año), un año.

1. Levanta la API escuchando en la red local (en el repo `ponte-pilas-api`):
   ```bash
   dotnet run --project src/PontePilas.Api --urls http://0.0.0.0:5080
   ```
2. Busca la IP de tu Mac: `ipconfig getifaddr en0` (el iPhone debe estar en el mismo Wi‑Fi).
3. Compila e instala en el iPhone, cambiando la IP:
   ```bash
   dotnet build src/PontePilas.App -p:Plataforma=ios -f net10.0-ios -c Debug \
     -p:RuntimeIdentifier=ios-arm64 -p:ApiUrl=http://192.168.1.50:5080/ \
     -p:CodesignKey="Apple Development" -t:Run
   ```
4. La primera vez, en el iPhone: Ajustes → General → VPN y gestión de dispositivos → confía en tu certificado de desarrollador.

Para el simulador basta con: `dotnet build src/PontePilas.App -p:Plataforma=ios -f net10.0-ios -t:Run`.

### Notificaciones push

- **Android (Firebase):** crea un proyecto en https://console.firebase.google.com, agrega una app Android con el paquete `com.pontepilas.app` y descarga `google-services.json` a `src/PontePilas.App/Platforms/Android/`. No se sube a Git. Sin ese archivo la app compila y funciona, pero no recibe push.
- **iOS (APNs):** necesita Apple Developer ($99/año) con la capacidad *Push Notifications* en el App ID. Compila con `-p:HabilitarPush=true`. Con un Apple ID gratis no hay push en iOS.
- La app pide permiso al abrir y registra el token en la API (`POST /api/dispositivos`). Las llaves para enviar (cuenta de servicio de Firebase y llave .p8 de Apple) van en la API.

## Accesos

| Qué | Enlace | Se actualiza |
|---|---|---|
| Producción (web) | No aplica todavía (Azure Static Web Apps) | Al fusionar a `main` (con aprobación), cuando exista el secreto de despliegue |
| QA | No aplica | — |
| Previews de PR | No aplica | — |
| GitHub Actions | https://github.com/eduardothomas/ponte-pilas-app/actions | En cada push y PR |
| APK de prueba | Artefacto `android-apk` en cada ejecución del CI | En cada PR |
| Repositorio | https://github.com/eduardothomas/ponte-pilas-app | — |
| Backend (privado) | https://github.com/eduardothomas/ponte-pilas-api | — |
| Panel de hosting | No aplica todavía (Azure) | — |

## Contenido del proyecto

```
src/PontePilas.Shared/   Contratos (DTOs, enums), repeticiones, agenda, calendario y validación. Lo usa también la API.
src/PontePilas.UI/       Razor Class Library: pantallas, componentes, cliente HTTP y estilos (wwwroot/css/pilas.css)
src/PontePilas.Web/      Host Blazor WebAssembly (web estática para Azure Static Web Apps)
src/PontePilas.App/      Host .NET MAUI Blazor Hybrid (Android e iOS)
tests/PontePilas.Shared.Tests/  Pruebas xUnit de Shared
```

- **Hoy:** tareas pendientes agrupadas en Atrasadas, Hoy, Mañana, Próximos 7 días, Más adelante y Algún día; se marcan como hechas con un toque.
- **Calendario:** vista mensual con puntos por prioridad, incluye las repeticiones; al tocar un día se ven sus tareas y se puede agregar una para esa fecha.
- **Editor de tarea:** título, fecha y hora opcionales, prioridad, repetición (diaria, semanal, mensual, anual), recordatorios (a la hora, 15 min, 1 h, 1 día antes) y notas.
- **Prioridad:** define cómo insisten los avisos (Baja: push; Normal: + correo; Alta: + WhatsApp; Urgente: WhatsApp repetido). La lógica vive en la API privada.

## Pruebas y build

```bash
dotnet test tests/PontePilas.Shared.Tests
dotnet publish src/PontePilas.Web -c Release -o publicado/web
dotnet publish src/PontePilas.App -p:Plataforma=android -f net10.0-android -c Release -p:AndroidPackageFormats=apk
```

El CI corre las pruebas, publica la web, genera el APK de Android y compila iOS para simulador en cada PR.

## Despliegue
- **Web:** Azure Static Web Apps (plan Free), workflow `.github/workflows/desplegar-web.yml`. Se publica al fusionar a `main` con autorización de Eduardo, cuando existan el secreto `AZURE_STATIC_WEB_APPS_API_TOKEN` y la variable `API_URL`. Sin ellos, el workflow no despliega.
- **Móvil:** APK de prueba desde el CI. TestFlight y tiendas, pendiente (requiere cuentas de Apple Developer y Google Play).

## Flujo de cambios
Ramas de trabajo y PR según el Estándar de Desarrollo de Software de Eduardo (v1.2). Si cambia `PontePilas.Shared`, también hay que abrir el PR que actualiza el submódulo en `ponte-pilas-api`. Detalles para agentes en `AGENTS.md`.

## Volver a una versión estable
Revertir el commit del PR en `main` (`git revert <sha>`) y abrir un PR con la reversión. En Azure Static Web Apps también se puede volver a un despliegue anterior desde el portal.

## Licencia
[Business Source License 1.1](LICENSE). Puedes ver, modificar y usar el código para uso personal, educativo o de evaluación. No está permitido ofrecerlo como producto o servicio comercial. El 18 de octubre de 2030 pasa a Apache 2.0.
