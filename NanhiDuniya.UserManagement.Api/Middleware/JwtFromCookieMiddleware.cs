//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//public class JwtFromCookieMiddleware
//{
//    private readonly RequestDelegate _next;
//    private readonly IConfiguration _config;

//    public JwtFromCookieMiddleware(RequestDelegate next, IConfiguration config)
//    {
//        _next = next;
//        _config = config;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        var token = context.Request.Headers["Authorization"].FirstOrDefault();
//        if (token != null)
//        {
//            var principal = ValidateToken(token);
//            if (principal != null)
//            {
//                context.User = principal;
//                await _next(context);
//            }
//            else
//            {
//                context.Response.StatusCode = 401;
//                context.Response.StatusCode = 401;
//                context.Response.Headers.Add("WWW-Authenticate", "Bearer error=\"invalid_token\"");
//                context.Response.WriteAsync("{\"accessToken\":\"newAccessToken\"}");
//            }
//        }
//        else
//        {
//            context.Response.StatusCode = 401;
//            context.Response.StatusCode = 401;
//            context.Response.Headers.Add("WWW-Authenticate", "Bearer error=\"invalid_token\"");
//            context.Response.WriteAsync("{\"accessToken\":\"newAccessToken\"}");
//        }
//    }

//    private ClaimsPrincipal ValidateToken(string token)
//    {
//        try
//        {
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var key = Encoding.ASCII.GetBytes(_config["JwtSettings:Secret"]); // Get the secret key from config
//            var tokenValidationParams = new TokenValidationParameters
//            {
//                ValidateIssuerSigningKey = true,
//                IssuerSigningKey = new SymmetricSecurityKey(key),
//                ValidateIssuer = false,
//                ValidateAudience = false,
//                ClockSkew = TimeSpan.Zero
//            };

//            var principal = tokenHandler.ValidateToken(token, tokenValidationParams, out var validatedToken);
//            return principal;
//        }
//        catch (SecurityTokenException)
//        {
//            return null; // Return null if token is invalid
//        }
//    }
//}