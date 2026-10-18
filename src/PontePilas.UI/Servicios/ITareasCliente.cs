using PontePilas.Shared.Modelos;

namespace PontePilas.UI.Servicios;

/// <summary>Acceso a las tareas del usuario a través de la API.</summary>
public interface ITareasCliente
{
    Task<IReadOnlyList<TareaDto>> ListarAsync(bool incluirCompletadas = false, CancellationToken ct = default);

    Task<TareaDto?> ObtenerAsync(Guid id, CancellationToken ct = default);

    Task<TareaDto> CrearAsync(GuardarTareaRequest request, CancellationToken ct = default);

    Task<TareaDto> ActualizarAsync(Guid id, GuardarTareaRequest request, CancellationToken ct = default);

    Task<TareaDto> CompletarAsync(Guid id, CancellationToken ct = default);

    Task<TareaDto> ReabrirAsync(Guid id, CancellationToken ct = default);

    Task EliminarAsync(Guid id, CancellationToken ct = default);
}
