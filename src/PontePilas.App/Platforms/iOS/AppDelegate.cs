using Foundation;
using PontePilas.App.Servicios;
using PontePilas.Shared.Modelos;
using UIKit;
using UserNotifications;

namespace PontePilas.App;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication application, NSDictionary? launchOptions)
    {
        var resultado = base.FinishedLaunching(application, launchOptions);

        UNUserNotificationCenter.Current.Delegate = new DelegadoNotificaciones();
        UNUserNotificationCenter.Current.RequestAuthorization(
            UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound | UNAuthorizationOptions.Badge,
            (permitido, _) =>
            {
                if (permitido)
                {
                    InvokeOnMainThread(application.RegisterForRemoteNotifications);
                }
            });

        return resultado;
    }

    /// <summary>Apple entrega el token del dispositivo (requiere compilar con -p:HabilitarPush=true y una cuenta con Push).</summary>
    [Export("application:didRegisterForRemoteNotificationsWithDeviceToken:")]
    public void RegistradoParaNotificaciones(UIApplication application, NSData deviceToken) =>
        _ = RegistroPush.RegistrarAsync(Convert.ToHexString(deviceToken.ToArray()).ToLowerInvariant(), PlataformaDispositivo.iOS);

    [Export("application:didFailToRegisterForRemoteNotificationsWithError:")]
    public void FalloRegistroNotificaciones(UIApplication application, NSError error) =>
        System.Diagnostics.Debug.WriteLine($"[Push] iOS no registró notificaciones: {error.LocalizedDescription}");

    /// <summary>Muestra la notificación aunque la app esté abierta.</summary>
    private sealed class DelegadoNotificaciones : UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(
            UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler) =>
            completionHandler(UNNotificationPresentationOptions.Banner | UNNotificationPresentationOptions.Sound | UNNotificationPresentationOptions.List);
    }
}
