using PontePilas.Shared.Agenda;
using PontePilas.Shared.Modelos;

namespace PontePilas.Shared.Tests;

public class AgendaTests
{
    private static readonly DateOnly Hoy = new(2026, 11, 1);

    private static TareaDto Tarea(
        string titulo,
        DateOnly? fecha = null,
        TimeOnly? hora = null,
        Prioridad prioridad = Prioridad.Normal,
        Repeticion repeticion = Repeticion.Ninguna,
        bool completada = false) =>
        new(Guid.NewGuid(), titulo, null, fecha, hora, prioridad, repeticion, completada, null, []);

    [Fact]
    public void Agrupa_en_secciones_y_omite_las_vacias_y_completadas()
    {
        var tareas = new[]
        {
            Tarea("Atrasada", Hoy.AddDays(-2)),
            Tarea("De hoy", Hoy),
            Tarea("De mañana", Hoy.AddDays(1)),
            Tarea("Esta semana", Hoy.AddDays(5)),
            Tarea("Lejana", Hoy.AddDays(30)),
            Tarea("Sin fecha"),
            Tarea("Ya hecha", Hoy, completada: true),
        };

        var secciones = AgendaBuilder.Construir(tareas, Hoy);

        Assert.Equal(
            [TipoSeccion.Atrasadas, TipoSeccion.Hoy, TipoSeccion.Manana, TipoSeccion.EstaSemana, TipoSeccion.MasAdelante, TipoSeccion.AlgunDia],
            secciones.Select(s => s.Tipo));
        Assert.Equal(["De hoy"], secciones.Single(s => s.Tipo == TipoSeccion.Hoy).Tareas.Select(t => t.Titulo));
    }

    [Fact]
    public void Ordena_por_hora_y_deja_al_final_las_que_no_tienen_hora()
    {
        var tareas = new[]
        {
            Tarea("Sin hora urgente", Hoy, prioridad: Prioridad.Urgente),
            Tarea("Tarde", Hoy, new TimeOnly(15, 0)),
            Tarea("Mañana temprano", Hoy, new TimeOnly(8, 0)),
        };

        var hoy = AgendaBuilder.Construir(tareas, Hoy).Single();

        Assert.Equal(["Mañana temprano", "Tarde", "Sin hora urgente"], hoy.Tareas.Select(t => t.Titulo));
    }

    [Fact]
    public void Con_la_misma_hora_va_primero_la_mas_prioritaria()
    {
        var tareas = new[] { Tarea("Normal", Hoy), Tarea("Alta", Hoy, prioridad: Prioridad.Alta) };

        Assert.Equal(["Alta", "Normal"], AgendaBuilder.Construir(tareas, Hoy).Single().Tareas.Select(t => t.Titulo));
    }

    [Fact]
    public void Calendario_de_noviembre_2026_empieza_el_lunes_26_de_octubre()
    {
        var semanas = MesCalendario.Construir(2026, 11, []);

        Assert.Equal(new DateOnly(2026, 10, 26), semanas[0][0].Fecha);
        Assert.False(semanas[0][0].EsDelMes);
        Assert.Equal(new DateOnly(2026, 11, 30), semanas[^1][0].Fecha);
        Assert.All(semanas, s => Assert.Equal(7, s.Count));
    }

    [Fact]
    public void Calendario_muestra_las_repeticiones_y_las_tareas_puntuales()
    {
        var tareas = new[]
        {
            Tarea("Gimnasio", new DateOnly(2026, 11, 2), repeticion: Repeticion.Semanal),
            Tarea("Pagar tarjeta", new DateOnly(2026, 11, 15)),
            Tarea("Sin fecha"),
        };

        var dias = MesCalendario.Construir(2026, 11, tareas).SelectMany(s => s).ToList();

        Assert.Equal(5, dias.Count(d => d.Tareas.Any(t => t.Titulo == "Gimnasio")));
        Assert.Equal(new DateOnly(2026, 11, 15), dias.Single(d => d.Tareas.Any(t => t.Titulo == "Pagar tarjeta")).Fecha);
        Assert.DoesNotContain(dias, d => d.Tareas.Any(t => t.Titulo == "Sin fecha"));
    }
}
