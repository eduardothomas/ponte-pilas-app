using System.Net.Http.Json;
using PontePilas.Shared;
using PontePilas.Shared.Modelos;

namespace PontePilas.App.Servicios;

/// <summary>Envía a la API el token de notificaciones de este teléfono.</summary>
public static class RegistroPush
{
    private static readonly HttpClient Http = new() { BaseAddress = ConfiguracionApi.Url, Timeout = TimeSpan.FromSeconds(20) };

    public static async Task RegistrarAsync(string token, PlataformaDispositivo plataforma)
    {
        try
        {
            using var respuesta = await Http.PostAsJsonAsync(
                "api/dispositivos", new RegistrarDispositivoRequest(token, plataforma), PontePilasJson.Opciones);
            System.Diagnostics.Debug.WriteLine($"[Push] Registro de {plataforma}: {(int)respuesta.StatusCode}");
        }
        catch (Exception ex)
        {
            // Sin conexión no pasa nada: se vuelve a registrar la próxima vez que se abra la app.
            System.Diagnostics.Debug.WriteLine($"[Push] No se pudo registrar el dispositivo: {ex.Message}");
        }
    }
}
