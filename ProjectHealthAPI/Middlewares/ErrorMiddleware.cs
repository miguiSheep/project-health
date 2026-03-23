using System.Net;
using System.Text.Json;

namespace ProjectHealthAPI.Middlewares
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Permite que la petición continúe su camino normal hacia el controlador
                await _next(context);
            }
            catch (Exception ex)
            {
                // Si algo explota en el camino, se atrapa aquí
                await ManejarExcepcionAsync(context, ex);
            }
        }

        private static Task ManejarExcepcionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Se estructura una respuesta estándar para evitar que Flutter reciba texto crudo
            var respuesta = new
            {
                Codigo = context.Response.StatusCode,
                Mensaje = "Ha ocurrido un error interno en el servidor local. Contacte al soporte técnico.",
                Detalle = exception.Message 
            };

            var resultado = JsonSerializer.Serialize(respuesta);
            return context.Response.WriteAsync(resultado);
        }
    }
}