using PontePilas.Shared.Modelos;

namespace PontePilas.Shared.Tests;

public class ModelosTests
{
    [Fact]
    public void GuardarTareaRequest_tiene_valores_por_defecto_sensatos()
    {
        var request = new GuardarTareaRequest("Pagar la luz");

        Assert.Null(request.Fecha);
        Assert.Equal(Prioridad.Normal, request.Prioridad);
        Assert.Equal(Repeticion.Ninguna, request.Repeticion);
    }
}
