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
            if (context.User?.Identity != null)
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    var username = context.User.Identity.Name;
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
