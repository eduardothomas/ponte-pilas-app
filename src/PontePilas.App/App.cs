using Microsoft.AspNetCore.Components.WebView.Maui;
using PontePilas.UI;

namespace PontePilas.App;

public class App : Application
{
    protected override Window CreateWindow(IActivationState? activationState) =>
        new(new MainPage()) { Title = "Ponte Pilas" };
}

public class MainPage : ContentPage
{
    public MainPage()
    {
        var blazor = new BlazorWebView { HostPage = "wwwroot/index.html" };
        blazor.RootComponents.Add(new RootComponent { Selector = "#app", ComponentType = typeof(Routes) });
        Content = blazor;
    }
}
