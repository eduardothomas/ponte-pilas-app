using System.Reflection;

namespace PontePilas.App;

/// <summary>
/// URL de la API. Se puede fijar al compilar con <c>-p:ApiUrl=http://192.168.1.50:5080/</c>
/// (necesario en un teléfono físico, donde "localhost" es el propio teléfono).
/// </summary>
public static class ConfiguracionApi
{
    public static readonly Uri Url = new(DesdeCompilacion() ?? PorDefecto);

#if DEBUG && ANDROID
    // El emulador de Android ve la máquina anfitriona como 10.0.2.2.
    private const string PorDefecto = "http://10.0.2.2:5080/";
#elif DEBUG
    // Simulador de iOS: comparte la red de la Mac.
    private const string PorDefecto = "http://localhost:5080/";
#else
    // Se reemplaza por la URL de Azure App Service cuando exista el entorno de producción.
    private const string PorDefecto = "https://ponte-pilas-api.azurewebsites.net/";
#endif

    private static string? DesdeCompilacion() =>
        typeof(ConfiguracionApi).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(a => a.Key == "ApiUrl")?.Value is { Length: > 0 } url ? url : null;
}
