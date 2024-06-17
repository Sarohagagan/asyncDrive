
namespace asyncDrive.Web.Middlewares
{
    public class AuthTokenHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthTokenHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!string.IsNullOrWhiteSpace(context.Session.GetString("AccessToken")))
            {
                var token = context.Session.GetString("AccessToken");
                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + token);
                }
            }
            await _next(context);
        }
    }
    public static class AuthTokenHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthTokenHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthTokenHandlerMiddleware>();
        }
    }
}
