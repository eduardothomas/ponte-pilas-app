using System.Globalization;
using PontePilas.Shared.Modelos;

namespace PontePilas.UI.Servicios;

/// <summary>Textos en español para fechas, horas y opciones.</summary>
public static class Formato
{
    public static readonly CultureInfo Es = CultureInfo.GetCultureInfo("es-PA");

    public static string Hora(TimeOnly hora) => hora.ToString("h:mm tt", Es).Replace(". ", "").Replace(".", "");

    public static string FechaCorta(DateOnly fecha, DateOnly hoy) => (fecha.DayNumber - hoy.DayNumber) switch
    {
        0 => "Hoy",
        1 => "Mañana",
        -1 => "Ayer",
        _ when fecha.Year == hoy.Year => Capitalizar(fecha.ToString("ddd d MMM", Es)),
        _ => Capitalizar(fecha.ToString("d MMM yyyy", Es)),
    };

    public static string Mes(int anio, int mes) => Capitalizar(new DateOnly(anio, mes, 1).ToString("MMMM yyyy", Es));

    public static string Repeticion(Repeticion r) => r switch
    {
        Shared.Modelos.Repeticion.Diaria => "Cada día",
        Shared.Modelos.Repeticion.Semanal => "Cada semana",
        Shared.Modelos.Repeticion.Mensual => "Cada mes",
        Shared.Modelos.Repeticion.Anual => "Cada año",
        _ => "No se repite",
    };

    public static string Prioridad(Prioridad p) => p switch
    {
        Shared.Modelos.Prioridad.Baja => "Baja",
        Shared.Modelos.Prioridad.Alta => "Alta",
        Shared.Modelos.Prioridad.Urgente => "Urgente",
        _ => "Normal",
    };

    public static string Aviso(int minutosAntes) => minutosAntes switch
    {
        0 => "A la hora",
        < 60 => $"{minutosAntes} min antes",
        60 => "1 hora antes",
        < 1440 when minutosAntes % 60 == 0 => $"{minutosAntes / 60} horas antes",
        1440 => "1 día antes",
        _ when minutosAntes % 1440 == 0 => $"{minutosAntes / 1440} días antes",
        _ => $"{minutosAntes} min antes",
    };

    private static string Capitalizar(string texto) =>
        string.IsNullOrEmpty(texto) ? texto : char.ToUpper(texto[0], Es) + texto[1..].Replace(".", "");
}
