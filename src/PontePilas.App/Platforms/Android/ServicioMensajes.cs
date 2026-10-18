using Android.App;
using AndroidX.Core.App;
using Firebase.Messaging;
using PontePilas.App.Servicios;
using PontePilas.Shared.Modelos;

namespace PontePilas.App;

/// <summary>Recibe tokens y mensajes de Firebase Cloud Messaging.</summary>
[Service(Exported = false)]
[IntentFilter(["com.google.firebase.MESSAGING_EVENT"])]
public class ServicioMensajes : FirebaseMessagingService
{
    public const string Canal = "recordatorios";

    public override void OnNewToken(string token) =>
        _ = RegistroPush.RegistrarAsync(token, PlataformaDispositivo.Android);

    /// <summary>Con la app abierta, Android no muestra la notificación solo: la mostramos aquí.</summary>
    public override void OnMessageReceived(RemoteMessage mensaje)
    {
        var notificacion = mensaje.GetNotification();
        if (notificacion is null)
        {
            return;
        }

        var construida = new NotificationCompat.Builder(this, Canal)
            .SetSmallIcon(Resource.Mipmap.appicon)
            .SetContentTitle(notificacion.Title)
            .SetContentText(notificacion.Body)
            .SetAutoCancel(true)
            .SetPriority(NotificationCompat.PriorityHigh)
            .Build();
        NotificationManagerCompat.From(this).Notify(Random.Shared.Next(), construida);
    }
}
