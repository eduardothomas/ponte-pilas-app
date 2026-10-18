using System.Globalization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PontePilas.UI;
using PontePilas.UI.Servicios;

CultureInfo.DefaultThreadCurrentCulture = Formato.Es;
CultureInfo.DefaultThreadCurrentUICulture = Formato.Es;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<Routes>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// La URL de la API viene de wwwroot/appsettings.json (o appsettings.{Entorno}.json).
var urlApi = builder.Configuration["ApiUrl"] ?? builder.HostEnvironment.BaseAddress;
builder.Services.AddPontePilasUI(new Uri(urlApi));

await builder.Build().RunAsync();
