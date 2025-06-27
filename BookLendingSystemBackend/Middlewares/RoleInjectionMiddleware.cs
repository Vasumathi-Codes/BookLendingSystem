using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BookLendingSystem.Middlewares
{
    public class RoleInjectionMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleInjectionMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            var role = context.Request.Headers["X-User-Role"].ToString();
            var name = context.Request.Headers["X-User-Name"].ToString();

            if (!string.IsNullOrWhiteSpace(role) && !string.IsNullOrWhiteSpace(name))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Role, role)
                };

                var identity = new ClaimsIdentity(claims, "CustomHeader");
                context.User = new ClaimsPrincipal(identity);
            }

            await _next(context);
        }
    }
}