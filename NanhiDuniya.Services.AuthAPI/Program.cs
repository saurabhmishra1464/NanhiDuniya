using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using Serilog;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Microsoft.AspNetCore.Mvc;

using NanhiDuniya.Services.AuthAPI.Data;
using NanhiDuniya.MessageBus.MassTransit;
using NanhiDuniya.Services.AuthAPI.Models;
using NanhiDuniya.Services.AuthAPI.Models.Dto;
using NanhiDuniya.Services.AuthAPI.Service;
using NanhiDuniya.Services.AuthAPI.Middleware;
using NanhiDuniya.Services.AuthAPI.Constants;
using static NanhiDuniya.Services.AuthAPI.Settings.NanhiDuniyaServices;
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
var jwtSettings = new JwtSettings();
configuration.Bind("JwtSettings", jwtSettings);
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddControllers();

// Register MassTransit with RabbitMQ
builder.Services.AddMassTransitWithRabbitMq();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
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
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var errors = actionContext.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToArray();

        var response = new ApiResponse<object>(
            success: false,
            message: "One or more validation errors occurred.",
            data: null,
            statusCode: StatusCodes.Status400BadRequest
        );

        return new ObjectResult(response)
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    };
});

//Sql Server Setup
 if (builder.Environment.IsProduction())
            {
                Console.WriteLine("--> Using azure SqlServer Db");
    builder.Services.AddDbContext<NanhiDuniyaDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("NanhiDuniyaAuthConn")));
}
else
{
    Console.WriteLine("--> local Db");
    builder.Services.AddDbContext<NanhiDuniyaDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
}

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));
//initializing services.
builder.Services.DataServiceCollection(builder.Configuration);

//Identity setting
builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("NanhiDuniyaUserManagementAPI")
    .AddEntityFrameworkStores<NanhiDuniyaDbContext>()
    .AddDefaultTokenProviders();

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
    options.Events.OnMessageReceived = context =>
    {

        if (context.Request.Cookies.ContainsKey("X-Access-Token"))
        {
            context.Token = context.Request.Cookies["X-Access-Token"];
        }

        return Task.CompletedTask;
    };
})
     .AddCookie(options =>
     {
         options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
         options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
         options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
         options.Cookie.IsEssential = true;
     });


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder
            .WithOrigins("https://localhost:7777","http://nanhiduniya.com","https://nanhiduniyafrontend.saurabhmishra.com")  // Allow requests from frontend
            .AllowAnyHeader()                     // Allow headers like Authorization
            .AllowAnyMethod()                     // Allow GET, POST, PUT, etc.
            .AllowCredentials());                 // Allow cookies (if needed)
});

builder.Services.Configure<JWTService>(configuration.GetSection("JwtSettings"));
builder.Services.Configure<NanhiDuniyaServicesSettings>(configuration.GetSection("NanhiDuniyaServices"));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Nanhi Duniya User Management API V1");
    options.RoutePrefix = string.Empty; // This will serve the Swagger UI at the root URL
});
app.MapGet("/", () => Results.Redirect("/index.html"));
app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionMiddleware>();
//app.UseHttpsRedirection(); // commented it for k8s
app.UseCors("AllowFrontend");
app.UseCookiePolicy();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<NanhiDuniyaDbContext>();
    if (app.Environment.IsProduction())
    {
        Console.WriteLine("--> Attempting to apply migrations...");
        try
        {
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not run migrations: {ex.Message}");
           
        }
    }
}
app.Run();

