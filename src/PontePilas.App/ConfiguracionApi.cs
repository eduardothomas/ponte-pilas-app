namespace PontePilas.App;

/// <summary>URL de la API. En desarrollo apunta a la máquina local; cambiar al publicar.</summary>
public static class ConfiguracionApi
{
#if DEBUG && ANDROID
    // El emulador de Android ve la máquina anfitriona como 10.0.2.2.
    public static readonly Uri Url = new("http://10.0.2.2:5080/");
#elif DEBUG
    public static readonly Uri Url = new("http://localhost:5080/");
#else
    // Se reemplaza por la URL de Azure App Service cuando exista el entorno de producción.
    public static readonly Uri Url = new("https://ponte-pilas-api.azurewebsites.net/");
#endif
}
