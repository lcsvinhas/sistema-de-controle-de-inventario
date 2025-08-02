using SistemaControleInventario.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace SistemaControleInventario.UserInterface.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public ExceptionMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (ProdutoException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(exception.Message);

            return context.Response.WriteAsync(result);
        }
    }
}
