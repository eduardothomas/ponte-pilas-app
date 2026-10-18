using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
#if FIREBASE_CONFIGURADO
using Android.Gms.Extensions;
#endif
using PontePilas.App.Servicios;
using PontePilas.Shared.Modelos;

namespace PontePilas.App;

[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        PrepararNotificaciones();
    }

    private void PrepararNotificaciones()
    {
        if (OperatingSystem.IsAndroidVersionAtLeast(26))
        {
            var canal = new NotificationChannel(ServicioMensajes.Canal, "Recordatorios", NotificationImportance.High)
            {
                Description = "Avisos de tus tareas",
            };
            ((NotificationManager)GetSystemService(NotificationService)!).CreateNotificationChannel(canal);
        }

        if (OperatingSystem.IsAndroidVersionAtLeast(33)
            && CheckSelfPermission(Manifest.Permission.PostNotifications) != Permission.Granted)
        {
            RequestPermissions([Manifest.Permission.PostNotifications], 1);
        }

#if FIREBASE_CONFIGURADO
        _ = RegistrarTokenAsync();
#endif
    }

#if FIREBASE_CONFIGURADO
    private static async Task RegistrarTokenAsync()
    {
        try
        {
            var token = await Firebase.Messaging.FirebaseMessaging.Instance.GetToken().AsAsync<Java.Lang.String>();
            await RegistroPush.RegistrarAsync(token.ToString(), PlataformaDispositivo.Android);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[Push] Firebase no dio token: {ex.Message}");
        }
    }
#endif
}
