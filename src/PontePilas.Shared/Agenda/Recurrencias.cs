using PontePilas.Shared.Modelos;

namespace PontePilas.Shared.Agenda;

/// <summary>Cálculo de fechas de tareas que se repiten.</summary>
/// <remarks>
/// Cada ocurrencia se calcula desde la fecha base (no encadenada), así una tarea mensual
/// del 31 cae el 30 en meses de 30 días y vuelve al 31 cuando el mes lo tiene.
/// </remarks>
public static class Recurrencias
{
    private const int MaxOcurrencias = 5000;

    /// <summary>La ocurrencia número <paramref name="n"/> (0 = la fecha base).</summary>
    public static DateOnly Ocurrencia(DateOnly fechaBase, Repeticion repeticion, int n) => repeticion switch
    {
        Repeticion.Ninguna => fechaBase,
        Repeticion.Diaria => fechaBase.AddDays(n),
        Repeticion.Semanal => fechaBase.AddDays(7 * n),
        Repeticion.Mensual => fechaBase.AddMonths(n),
        Repeticion.Anual => fechaBase.AddYears(n),
        _ => throw new ArgumentOutOfRangeException(nameof(repeticion), repeticion, null),
    };

    /// <summary>Todas las ocurrencias entre <paramref name="desde"/> y <paramref name="hasta"/>, ambas incluidas.</summary>
    public static IEnumerable<DateOnly> Ocurrencias(DateOnly fechaBase, Repeticion repeticion, DateOnly desde, DateOnly hasta)
    {
        if (hasta < desde || hasta < fechaBase)
        {
            yield break;
        }

        if (repeticion == Repeticion.Ninguna)
        {
            if (fechaBase >= desde)
            {
                yield return fechaBase;
            }

            yield break;
        }

        for (var n = IndiceInicial(fechaBase, repeticion, desde); n < MaxOcurrencias; n++)
        {
            var fecha = Ocurrencia(fechaBase, repeticion, n);
            if (fecha > hasta)
            {
                yield break;
            }

            if (fecha >= desde)
            {
                yield return fecha;
            }
        }
    }

    /// <summary>
    /// Fecha a la que pasa una tarea repetitiva al completarse: la primera ocurrencia posterior a la
    /// actual que no esté en el pasado. Si la tarea no se repite, devuelve null.
    /// </summary>
    public static DateOnly? SiguienteAlCompletar(DateOnly fechaActual, Repeticion repeticion, DateOnly fechaBase, DateOnly hoy)
    {
        if (repeticion == Repeticion.Ninguna)
        {
            return null;
        }

        var minimo = fechaActual >= hoy ? fechaActual.AddDays(1) : hoy;
        return Ocurrencias(fechaBase, repeticion, minimo, minimo.AddYears(2)).Cast<DateOnly?>().FirstOrDefault();
    }

    private static int IndiceInicial(DateOnly fechaBase, Repeticion repeticion, DateOnly desde)
    {
        if (desde <= fechaBase)
        {
            return 0;
        }

        // Estimación por debajo para no recorrer años de ocurrencias.
        var dias = desde.DayNumber - fechaBase.DayNumber;
        var n = repeticion switch
        {
            Repeticion.Diaria => dias,
            Repeticion.Semanal => dias / 7,
            Repeticion.Mensual => ((desde.Year - fechaBase.Year) * 12) + desde.Month - fechaBase.Month - 1,
            Repeticion.Anual => desde.Year - fechaBase.Year - 1,
            _ => 0,
        };
        return Math.Max(0, n);
    }
}
