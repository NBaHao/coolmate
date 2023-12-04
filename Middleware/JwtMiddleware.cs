using CoolMate.Services;
using System.Net;

namespace CoolMate.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var authService = context.RequestServices.GetRequiredService<AuthService>();

            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var isBlacklisted = await authService.IsTokenBlacklistedAsync(token);
            if (isBlacklisted)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            await _next(context);
        }
    }
}
