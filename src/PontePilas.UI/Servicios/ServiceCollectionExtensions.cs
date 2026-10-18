using Microsoft.Extensions.DependencyInjection;

namespace PontePilas.UI.Servicios;

public static class ServiceCollectionExtensions
{
    /// <summary>Registra la UI de Ponte Pilas apuntando a la API en <paramref name="urlApi"/>.</summary>
    public static IServiceCollection AddPontePilasUI(this IServiceCollection services, Uri urlApi)
    {
        services.AddScoped<ITareasCliente>(_ => new TareasApiCliente(new HttpClient { BaseAddress = urlApi }));
        return services;
    }
}
