using PontePilas.Shared.Agenda;
using PontePilas.Shared.Modelos;

namespace PontePilas.Shared.Tests;

public class RecurrenciasTests
{
    private static DateOnly F(int anio, int mes, int dia) => new(anio, mes, dia);

    [Fact]
    public void Sin_repeticion_solo_devuelve_la_fecha_si_cae_en_el_rango()
    {
        Assert.Equal([F(2026, 11, 3)], Recurrencias.Ocurrencias(F(2026, 11, 3), Repeticion.Ninguna, F(2026, 11, 1), F(2026, 11, 30)));
        Assert.Empty(Recurrencias.Ocurrencias(F(2026, 10, 3), Repeticion.Ninguna, F(2026, 11, 1), F(2026, 11, 30)));
    }

    [Fact]
    public void Semanal_devuelve_cada_siete_dias_dentro_del_rango()
    {
        var fechas = Recurrencias.Ocurrencias(F(2026, 10, 5), Repeticion.Semanal, F(2026, 11, 1), F(2026, 11, 30)).ToList();

        Assert.Equal([F(2026, 11, 2), F(2026, 11, 9), F(2026, 11, 16), F(2026, 11, 23), F(2026, 11, 30)], fechas);
    }

    [Fact]
    public void Mensual_el_31_se_ajusta_al_ultimo_dia_y_vuelve_al_31()
    {
        var fechas = Recurrencias.Ocurrencias(F(2026, 1, 31), Repeticion.Mensual, F(2026, 1, 1), F(2026, 5, 31)).ToList();

        Assert.Equal([F(2026, 1, 31), F(2026, 2, 28), F(2026, 3, 31), F(2026, 4, 30), F(2026, 5, 31)], fechas);
    }

    [Fact]
    public void No_hay_ocurrencias_antes_de_la_fecha_base()
    {
        var fechas = Recurrencias.Ocurrencias(F(2026, 11, 15), Repeticion.Diaria, F(2026, 11, 1), F(2026, 11, 16)).ToList();

        Assert.Equal([F(2026, 11, 15), F(2026, 11, 16)], fechas);
    }

    [Fact]
    public void Diaria_con_rango_lejano_no_recorre_desde_el_inicio()
    {
        var fechas = Recurrencias.Ocurrencias(F(2020, 1, 1), Repeticion.Diaria, F(2030, 6, 1), F(2030, 6, 3)).ToList();

        Assert.Equal([F(2030, 6, 1), F(2030, 6, 2), F(2030, 6, 3)], fechas);
    }

    [Fact]
    public void Al_completar_a_tiempo_pasa_a_la_siguiente_ocurrencia()
    {
        var hoy = F(2026, 11, 1);

        Assert.Equal(F(2026, 11, 8), Recurrencias.SiguienteAlCompletar(hoy, Repeticion.Semanal, F(2026, 10, 25), hoy));
    }

    [Fact]
    public void Al_completar_atrasada_no_deja_la_siguiente_en_el_pasado()
    {
        var hoy = F(2026, 11, 10);

        Assert.Equal(F(2026, 11, 10), Recurrencias.SiguienteAlCompletar(F(2026, 11, 3), Repeticion.Diaria, F(2026, 11, 1), hoy));
        Assert.Equal(F(2026, 11, 15), Recurrencias.SiguienteAlCompletar(F(2026, 11, 1), Repeticion.Semanal, F(2026, 11, 1), hoy));
    }

    [Fact]
    public void Al_completar_sin_repeticion_devuelve_null()
    {
        Assert.Null(Recurrencias.SiguienteAlCompletar(F(2026, 11, 1), Repeticion.Ninguna, F(2026, 11, 1), F(2026, 11, 1)));
    }
}
