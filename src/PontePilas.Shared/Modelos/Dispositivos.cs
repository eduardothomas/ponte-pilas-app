namespace PontePilas.Shared.Modelos;

/// <summary>De dónde viene el token de notificaciones push.</summary>
public enum PlataformaDispositivo
{
    /// <summary>Token de Firebase Cloud Messaging.</summary>
    Android = 0,

    /// <summary>Token de Apple Push Notification service (hexadecimal).</summary>
    iOS = 1,
}

/// <summary>Registra el teléfono para recibir notificaciones push.</summary>
public sealed record RegistrarDispositivoRequest(string Token, PlataformaDispositivo Plataforma);
