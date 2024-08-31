namespace NanhiDuniya.UserManagement.Api.Middleware
{
    public class JwtFromCookieMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtFromCookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Cookies.TryGetValue("accessToken", out var token))
            {
                context.Request.Headers.Add("Authorization", $"Bearer {token}");
            }

            await _next(context);
        }
    }

}
