namespace PontePilas.Shared.Modelos;

/// <summary>Una tarea tal como la devuelve la API.</summary>
/// <param name="Fecha">Día en que toca. Null = "algún día" (sin fecha).</param>
/// <param name="Hora">Hora local opcional. Null = en cualquier momento del día.</param>
/// <param name="AvisosMinutosAntes">Minutos antes de la fecha/hora en que se avisa (0 = a la hora).</param>
public sealed record TareaDto(
    Guid Id,
    string Titulo,
    string? Notas,
    DateOnly? Fecha,
    TimeOnly? Hora,
    Prioridad Prioridad,
    Repeticion Repeticion,
    bool Completada,
    DateTimeOffset? CompletadaEn,
    IReadOnlyList<int> AvisosMinutosAntes);

/// <summary>Datos para crear o actualizar una tarea.</summary>
public sealed record GuardarTareaRequest(
    string Titulo,
    string? Notas = null,
    DateOnly? Fecha = null,
    TimeOnly? Hora = null,
    Prioridad Prioridad = Prioridad.Normal,
    Repeticion Repeticion = Repeticion.Ninguna,
    IReadOnlyList<int>? AvisosMinutosAntes = null);
