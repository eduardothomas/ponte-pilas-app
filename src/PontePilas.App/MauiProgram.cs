using System.Globalization;
using Microsoft.Extensions.Logging;
using PontePilas.UI.Servicios;

namespace PontePilas.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        CultureInfo.DefaultThreadCurrentCulture = Formato.Es;
        CultureInfo.DefaultThreadCurrentUICulture = Formato.Es;

        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddPontePilasUI(ConfiguracionApi.Url);

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.SetMinimumLevel(LogLevel.Debug);
#endif

        return builder.Build();
    }
}
