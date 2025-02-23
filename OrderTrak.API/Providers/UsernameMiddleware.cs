using System.Security.Claims;

namespace OrderTrak.API.Providers
{
    public class UsernameMiddleware
    {
        private readonly RequestDelegate _next;

        public UsernameMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var username = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                if (username != null)
                {
                    context.Items["Username"] = username;
                }
            }

            await _next(context);
        }
    }

    // Extension method to add the middleware
    public static class UsernameMiddlewareExtensions
    {
        public static IApplicationBuilder UseUsernameMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UsernameMiddleware>();
        }
    }
}
