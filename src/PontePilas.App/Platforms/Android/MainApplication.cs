using Android.App;
using Android.Runtime;

namespace PontePilas.App;

[Application(UsesCleartextTraffic = true)] // Solo para llamar a la API local en desarrollo (http).
public class MainApplication(IntPtr handle, JniHandleOwnership ownership) : MauiApplication(handle, ownership)
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
