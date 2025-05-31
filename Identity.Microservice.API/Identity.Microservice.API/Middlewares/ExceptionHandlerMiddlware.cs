using Identity.Microservice.API.Models.Response;
using Identity.Microservice.AppCore.Exceptions;

namespace Identity.Microservice.API.Middlewares
{
    public class ExceptionHandlerMiddlware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddlware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (HttpException httpEx)
            {
                await HttpExceptionHandler(httpContext, httpEx);
            }
            catch (Exception ex)
            {
                await ExceptionHandler(httpContext, ex);
            }
        }

        private async Task HttpExceptionHandler(HttpContext context, HttpException httpEx)
        {
            switch (httpEx.StatusCode)
            {
                default:
                    {
                        context.Response.StatusCode = (int)httpEx.StatusCode;
                        context.Response.ContentType = "application/json";


                        var response = new ErrorDetails((int)httpEx.StatusCode, httpEx.Message);

                        await context.Response.WriteAsJsonAsync(response);
                        break;
                    }
            }
        }

        private async Task ExceptionHandler(HttpContext context, Exception notFoundEx)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var response = new ErrorDetails(context.Response.StatusCode, notFoundEx.Message);

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
