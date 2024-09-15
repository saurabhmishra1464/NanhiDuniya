using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NanhiDuniya.Service.Services;
using NanhiDuniya.Data.Entities;
using NanhiDuniya.UserManagement.APi.Middleware;
using Serilog;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NanhiDuniya.Core.Interfaces;
using NanhiDuniya.Data.Repositories;
using NanhiDuniya.Core.Utilities;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
var jwtSettings = new JwtSettings();
configuration.Bind("JwtSettings", jwtSettings);
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Nanhi Duniya User Management API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            },
                Scheme = "0auth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

//Sql Server Setup
builder.Services.AddDbContext<NanhiDuniyaDbContext>(options =>options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

//initializing services.
builder.Services.DataServiceCollection(builder.Configuration);

//Identity setting
builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("NanhiDuniyaUserManagementAPI")
    .AddEntityFrameworkStores<NanhiDuniyaDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
        ClockSkew = jwtSettings.Expire
    };
    options.SaveToken = true;
    options.Events = new JwtBearerEvents();
    options.Events.OnMessageReceived = context => {

        if (context.Request.Cookies.ContainsKey("X-Access-Token"))
        {
            context.Token = context.Request.Cookies["X-Access-Token"];
        }

        return Task.CompletedTask;
    };
     })
     .AddCookie(options =>
     {
      options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
      options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
      options.Cookie.IsEssential = true;
     });


builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()); // This line is crucial
});
//builder.Services.AddCors(options =>
//  options.AddPolicy("Dev", builder =>
//  {
//      // Allow multiple methods
//      builder.WithMethods("GET", "POST", "PATCH", "DELETE", "OPTIONS")
//        .WithHeaders(
//          HeaderNames.Accept,
//          HeaderNames.ContentType,
//          HeaderNames.Authorization)
//        .AllowCredentials()
//        .SetIsOriginAllowed(origin =>
//        {
//            if (string.IsNullOrWhiteSpace(origin)) return false;
//            // Only add this to allow testing with localhost, remove this line in production!
//            if (origin.ToLower().StartsWith("http://localhost:3000")) return true;
//            // Insert your production domain here.
//            if (origin.ToLower().StartsWith("https://dev.mydomain.com")) return true;
//            return false;
//        });
//  })
//);
builder.Services.Configure<JWTService>(configuration.GetSection("JwtSettings"));
builder.Services.Configure<NanhiDuniyaServicesSettings>(configuration.GetSection("NanhiDuniyaServices"));



var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionMiddleware>();
//app.UseMiddleware<JwtFromCookieMiddleware>();
app.UseHttpsRedirection();
//app.Use(async (context, next) =>
//{
//    context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
//    context.Response.Headers.Add("Content-Security-Policy", "frame-ancestors 'none'");
//    context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
//    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
//    context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
//    await next();
//});
app.UseCors("CorsPolicy");
app.UseCookiePolicy();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

