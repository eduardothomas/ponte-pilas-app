using System.Net;
using System.Net.Http.Json;
using PontePilas.Shared;
using PontePilas.Shared.Modelos;

namespace PontePilas.UI.Servicios;

/// <summary>Implementación HTTP de <see cref="ITareasCliente"/> contra la API de Ponte Pilas.</summary>
public sealed class TareasApiCliente(HttpClient http) : ITareasCliente
{
    private const string Ruta = "api/tareas";

    public async Task<IReadOnlyList<TareaDto>> ListarAsync(bool incluirCompletadas = false, CancellationToken ct = default) =>
        await http.GetFromJsonAsync<List<TareaDto>>($"{Ruta}?incluirCompletadas={incluirCompletadas.ToString().ToLowerInvariant()}", PontePilasJson.Opciones, ct)
        ?? [];

    public async Task<TareaDto?> ObtenerAsync(Guid id, CancellationToken ct = default)
    {
        using var respuesta = await http.GetAsync($"{Ruta}/{id}", ct);
        if (respuesta.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        return await LeerAsync(respuesta, ct);
    }

    public async Task<TareaDto> CrearAsync(GuardarTareaRequest request, CancellationToken ct = default)
    {
        using var respuesta = await http.PostAsJsonAsync(Ruta, request, PontePilasJson.Opciones, ct);
        return await LeerAsync(respuesta, ct);
    }

    public async Task<TareaDto> ActualizarAsync(Guid id, GuardarTareaRequest request, CancellationToken ct = default)
    {
        using var respuesta = await http.PutAsJsonAsync($"{Ruta}/{id}", request, PontePilasJson.Opciones, ct);
        return await LeerAsync(respuesta, ct);
    }

    public async Task<TareaDto> CompletarAsync(Guid id, CancellationToken ct = default)
    {
        using var respuesta = await http.PostAsync($"{Ruta}/{id}/completar", null, ct);
        return await LeerAsync(respuesta, ct);
    }

    public async Task<TareaDto> ReabrirAsync(Guid id, CancellationToken ct = default)
    {
        using var respuesta = await http.PostAsync($"{Ruta}/{id}/reabrir", null, ct);
        return await LeerAsync(respuesta, ct);
    }

    public async Task EliminarAsync(Guid id, CancellationToken ct = default)
    {
        using var respuesta = await http.DeleteAsync($"{Ruta}/{id}", ct);
        if (respuesta.StatusCode != HttpStatusCode.NotFound)
        {
            respuesta.EnsureSuccessStatusCode();
        }
    }

    private static async Task<TareaDto> LeerAsync(HttpResponseMessage respuesta, CancellationToken ct)
    {
        respuesta.EnsureSuccessStatusCode();
        return await respuesta.Content.ReadFromJsonAsync<TareaDto>(PontePilasJson.Opciones, ct)
            ?? throw new InvalidOperationException("La API devolvió una respuesta vacía.");
    }
}
