using Identity.Microservice.API.Middlewares;

namespace Identity.Microservice.API.Extensions
{
    public static class ExceptionHandlerMiddlwareExtension
    {
        public static IApplicationBuilder UseExceptionHandlers(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddlware>();
        }
    }
}
