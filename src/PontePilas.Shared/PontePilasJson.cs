using System.Text.Json;
using System.Text.Json.Serialization;

namespace PontePilas.Shared;

/// <summary>Opciones JSON que usan el cliente y la API (camelCase y enums como texto).</summary>
public static class PontePilasJson
{
    public static JsonSerializerOptions Opciones { get; } = Configurar(new JsonSerializerOptions(JsonSerializerDefaults.Web));

    public static JsonSerializerOptions Configurar(JsonSerializerOptions opciones)
    {
        opciones.Converters.Add(new JsonStringEnumConverter());
        return opciones;
    }
}
