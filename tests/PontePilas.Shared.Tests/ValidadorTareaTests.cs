using PontePilas.Shared.Modelos;
using PontePilas.Shared.Validacion;

namespace PontePilas.Shared.Tests;

public class ValidadorTareaTests
{
    private static readonly DateOnly Fecha = new(2026, 11, 1);

    [Fact]
    public void Tarea_minima_es_valida()
    {
        Assert.Empty(ValidadorTarea.Validar(new GuardarTareaRequest("Comprar pan")));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Titulo_vacio_no_es_valido(string titulo)
    {
        Assert.Contains(nameof(GuardarTareaRequest.Titulo), ValidadorTarea.Validar(new GuardarTareaRequest(titulo)).Keys);
    }

    [Fact]
    public void Hora_repeticion_o_avisos_sin_fecha_no_son_validos()
    {
        var errores = ValidadorTarea.Validar(new GuardarTareaRequest(
            "X", Hora: new TimeOnly(9, 0), Repeticion: Repeticion.Diaria, AvisosMinutosAntes: [0]));

        Assert.Contains(nameof(GuardarTareaRequest.Hora), errores.Keys);
        Assert.Contains(nameof(GuardarTareaRequest.Repeticion), errores.Keys);
        Assert.Contains(nameof(GuardarTareaRequest.AvisosMinutosAntes), errores.Keys);
    }

    [Fact]
    public void Avisos_negativos_o_demasiados_no_son_validos()
    {
        Assert.NotEmpty(ValidadorTarea.Validar(new GuardarTareaRequest("X", Fecha: Fecha, AvisosMinutosAntes: [-5])));
        Assert.NotEmpty(ValidadorTarea.Validar(new GuardarTareaRequest("X", Fecha: Fecha, AvisosMinutosAntes: [0, 5, 10, 15, 30, 60])));
    }

    [Fact]
    public void Normalizar_limpia_espacios_y_ordena_avisos_sin_duplicados()
    {
        var limpio = ValidadorTarea.Normalizar(new GuardarTareaRequest("  Pagar luz ", "   ", Fecha, AvisosMinutosAntes: [60, 0, 60]));

        Assert.Equal("Pagar luz", limpio.Titulo);
        Assert.Null(limpio.Notas);
        Assert.Equal([0, 60], limpio.AvisosMinutosAntes);
    }
}
