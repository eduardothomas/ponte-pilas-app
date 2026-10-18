namespace PontePilas.Shared.Modelos;

/// <summary>Qué tan importante es una tarea. Define cómo se escalan los avisos.</summary>
public enum Prioridad
{
    Baja = 0,
    Normal = 1,
    Alta = 2,
    Urgente = 3,
}

/// <summary>Cada cuánto se repite una tarea.</summary>
public enum Repeticion
{
    Ninguna = 0,
    Diaria = 1,
    Semanal = 2,
    Mensual = 3,
    Anual = 4,
}

/// <summary>Por dónde se envía un aviso.</summary>
public enum CanalAviso
{
    Push = 0,
    Correo = 1,
    WhatsApp = 2,
}
