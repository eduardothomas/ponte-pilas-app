using PontePilas.Shared.Modelos;

namespace PontePilas.Shared.Agenda;

public sealed record DiaCalendario(DateOnly Fecha, bool EsDelMes, IReadOnlyList<TareaDto> Tareas);

/// <summary>Arma la cuadrícula de un mes (semanas completas) con las tareas de cada día, repeticiones incluidas.</summary>
public static class MesCalendario
{
    public static IReadOnlyList<IReadOnlyList<DiaCalendario>> Construir(
        int anio,
        int mes,
        IEnumerable<TareaDto> tareas,
        DayOfWeek primerDiaSemana = DayOfWeek.Monday)
    {
        var primeroDelMes = new DateOnly(anio, mes, 1);
        var desfase = ((int)primeroDelMes.DayOfWeek - (int)primerDiaSemana + 7) % 7;
        var inicio = primeroDelMes.AddDays(-desfase);
        var ultimoDelMes = primeroDelMes.AddMonths(1).AddDays(-1);
        var semanas = (int)Math.Ceiling((desfase + ultimoDelMes.Day) / 7.0);
        var fin = inicio.AddDays((semanas * 7) - 1);

        var porDia = TareasPorDia(tareas, inicio, fin);

        return Enumerable.Range(0, semanas)
            .Select(s => (IReadOnlyList<DiaCalendario>)Enumerable.Range(0, 7)
                .Select(d =>
                {
                    var fecha = inicio.AddDays((s * 7) + d);
                    var delDia = porDia.TryGetValue(fecha, out var lista) ? lista : [];
                    return new DiaCalendario(fecha, fecha.Month == mes, AgendaBuilder.Ordenar(delDia).ToList());
                })
                .ToList())
            .ToList();
    }

    /// <summary>
    /// Tareas por día en un rango. Una tarea repetitiva aparece en cada ocurrencia a partir de su fecha
    /// actual; las ocurrencias futuras se muestran como pendientes.
    /// </summary>
    public static Dictionary<DateOnly, List<TareaDto>> TareasPorDia(IEnumerable<TareaDto> tareas, DateOnly desde, DateOnly hasta)
    {
        var porDia = new Dictionary<DateOnly, List<TareaDto>>();
        foreach (var tarea in tareas)
        {
            if (tarea.Fecha is not { } fecha)
            {
                continue;
            }

            var repeticion = tarea.Completada ? Repeticion.Ninguna : tarea.Repeticion;
            foreach (var dia in Recurrencias.Ocurrencias(fecha, repeticion, desde, hasta))
            {
                if (!porDia.TryGetValue(dia, out var lista))
                {
                    porDia[dia] = lista = [];
                }

                lista.Add(dia == fecha ? tarea : tarea with { Fecha = dia });
            }
        }

        return porDia;
    }
}
