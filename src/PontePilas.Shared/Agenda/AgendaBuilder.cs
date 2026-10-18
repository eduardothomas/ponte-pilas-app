using PontePilas.Shared.Modelos;

namespace PontePilas.Shared.Agenda;

public enum TipoSeccion
{
    Atrasadas,
    Hoy,
    Manana,
    EstaSemana,
    MasAdelante,
    AlgunDia,
}

public sealed record SeccionAgenda(TipoSeccion Tipo, string Titulo, IReadOnlyList<TareaDto> Tareas);

/// <summary>Agrupa las tareas pendientes en las secciones de la pantalla "Hoy".</summary>
public static class AgendaBuilder
{
    public static IReadOnlyList<SeccionAgenda> Construir(IEnumerable<TareaDto> tareas, DateOnly hoy)
    {
        var pendientes = tareas.Where(t => !t.Completada).ToList();
        var finSemana = hoy.AddDays(7);

        var grupos = new (TipoSeccion Tipo, string Titulo, Func<TareaDto, bool> Filtro)[]
        {
            (TipoSeccion.Atrasadas, "Atrasadas", t => t.Fecha < hoy),
            (TipoSeccion.Hoy, "Hoy", t => t.Fecha == hoy),
            (TipoSeccion.Manana, "Mañana", t => t.Fecha == hoy.AddDays(1)),
            (TipoSeccion.EstaSemana, "Próximos 7 días", t => t.Fecha > hoy.AddDays(1) && t.Fecha <= finSemana),
            (TipoSeccion.MasAdelante, "Más adelante", t => t.Fecha > finSemana),
            (TipoSeccion.AlgunDia, "Algún día", t => t.Fecha is null),
        };

        return grupos
            .Select(g => new SeccionAgenda(g.Tipo, g.Titulo, Ordenar(pendientes.Where(g.Filtro)).ToList()))
            .Where(s => s.Tareas.Count > 0)
            .ToList();
    }

    /// <summary>Orden dentro de una sección: fecha, hora (sin hora al final), prioridad más alta primero y título.</summary>
    public static IEnumerable<TareaDto> Ordenar(IEnumerable<TareaDto> tareas) => tareas
        .OrderBy(t => t.Fecha ?? DateOnly.MaxValue)
        .ThenBy(t => t.Hora ?? TimeOnly.MaxValue)
        .ThenByDescending(t => t.Prioridad)
        .ThenBy(t => t.Titulo, StringComparer.CurrentCultureIgnoreCase);
}
