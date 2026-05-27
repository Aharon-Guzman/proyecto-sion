using Sion.BLL.Exceptions;
using System.Net;

namespace Sion.PL.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (EntidadNoEncontradaException ex)
        {
            _logger.LogWarning(ex, "Entidad no encontrada: {Message}", ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.Redirect("/Error?code=404");
        }
        catch (OperacionNoPermitidaException ex)
        {
            _logger.LogWarning(ex, "Operación no permitida: {Message}", ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            context.Response.Redirect("/Error?code=403");
        }
        catch (ArchivoInvalidoException ex)
        {
            _logger.LogWarning(ex, "Archivo inválido: {Message}", ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.Redirect("/Error?code=400");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado: {Message}", ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.Redirect("/Error?code=500");
        }
    }
}