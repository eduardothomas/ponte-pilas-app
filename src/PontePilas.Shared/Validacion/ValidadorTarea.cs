using PontePilas.Shared.Modelos;

namespace PontePilas.Shared.Validacion;

/// <summary>Reglas de validación comunes a la UI y a la API.</summary>
public static class ValidadorTarea
{
    public const int LargoMaximoTitulo = 200;
    public const int LargoMaximoNotas = 4000;
    public const int MaximoAvisos = 5;
    public const int MaximoMinutosAntes = 60 * 24 * 30;

    /// <summary>Devuelve los errores por campo. Vacío si es válida.</summary>
    public static IReadOnlyDictionary<string, string> Validar(GuardarTareaRequest request)
    {
        var errores = new Dictionary<string, string>();

        if (string.IsNullOrWhiteSpace(request.Titulo))
        {
            errores[nameof(request.Titulo)] = "Escribe qué tienes que hacer.";
        }
        else if (request.Titulo.Trim().Length > LargoMaximoTitulo)
        {
            errores[nameof(request.Titulo)] = $"El título no puede pasar de {LargoMaximoTitulo} caracteres.";
        }

        if (request.Notas?.Length > LargoMaximoNotas)
        {
            errores[nameof(request.Notas)] = $"Las notas no pueden pasar de {LargoMaximoNotas} caracteres.";
        }

        if (request.Fecha is null && request.Hora is not null)
        {
            errores[nameof(request.Hora)] = "Para poner hora, primero elige una fecha.";
        }

        if (request.Fecha is null && request.Repeticion != Repeticion.Ninguna)
        {
            errores[nameof(request.Repeticion)] = "Una tarea que se repite necesita fecha de inicio.";
        }

        var avisos = request.AvisosMinutosAntes ?? [];
        if (avisos.Count > 0 && request.Fecha is null)
        {
            errores[nameof(request.AvisosMinutosAntes)] = "Los recordatorios necesitan una fecha.";
        }
        else if (avisos.Count > MaximoAvisos)
        {
            errores[nameof(request.AvisosMinutosAntes)] = $"Máximo {MaximoAvisos} recordatorios por tarea.";
        }
        else if (avisos.Any(m => m is < 0 or > MaximoMinutosAntes))
        {
            errores[nameof(request.AvisosMinutosAntes)] = "Los recordatorios deben ser entre la hora de la tarea y 30 días antes.";
        }

        return errores;
    }

    /// <summary>Limpia espacios y duplicados sin cambiar el significado.</summary>
    public static GuardarTareaRequest Normalizar(GuardarTareaRequest request) => request with
    {
        Titulo = request.Titulo.Trim(),
        Notas = string.IsNullOrWhiteSpace(request.Notas) ? null : request.Notas.Trim(),
        AvisosMinutosAntes = (request.AvisosMinutosAntes ?? []).Distinct().Order().ToList(),
    };
}
