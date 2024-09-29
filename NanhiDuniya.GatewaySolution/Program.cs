using Microsoft.AspNetCore.Server.Kestrel.Core;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.Values;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using NanhiDuniya.GatewaySolution;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//var configuration = builder.Configuration;
//// Add services to the container.
//var jwtSettings = new JwtSettings();
//configuration.Bind("JwtSettings", jwtSettings);
if (builder.Environment.EnvironmentName.ToString().ToLower().Equals("production"))
{
    builder.Configuration.AddJsonFile("ocelot.Production.json", optional: false, reloadOnChange: true);
}
else
{
    builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
}
builder.Services.AddOcelot(builder.Configuration);
//builder.Services.Configure<KestrelServerOptions>(options =>
//{
//    options.Listen(IPAddress.Any, 7777, listenOptions =>
//    {
//        listenOptions.UseHttps("C:\\Windows\\System32\\localhost.pfx", "@Sanu123*");
//    });
//});

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = jwtSettings.Issuer,
//        ValidAudience = jwtSettings.Audience,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
//        ClockSkew = jwtSettings.Expire
//    };
//    options.SaveToken = true;
//    options.Events = new JwtBearerEvents();
//    options.Events.OnMessageReceived = context =>
//    {

//        if (context.Request.Cookies.ContainsKey("X-Access-Token"))
//        {
//            context.Token = context.Request.Cookies["X-Access-Token"];
//        }

//        return Task.CompletedTask;
//    };
//})
//     .AddCookie(options =>
//     {
//         options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
//         options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//         options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//         options.Cookie.IsEssential = true;
//     });


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder
            .WithOrigins("https://localhost:3000")  // Allow requests from frontend
            .AllowAnyHeader()                     // Allow headers like Authorization
            .AllowAnyMethod()                     // Allow GET, POST, PUT, etc.
            .AllowCredentials());                 // Allow cookies (if needed)
});

var app = builder.Build();
app.UseCors("AllowFrontend");
app.MapGet("/", () => "Hello World!");
app.UseOcelot().GetAwaiter().GetResult();
app.Run();
