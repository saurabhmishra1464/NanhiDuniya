using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NanhiDuniya.Service.Services;
using NanhiDuniya.Data.Entities;
using NanhiDuniya.UserManagement.APi.Middleware;
using Serilog;
using Microsoft.AspNetCore.Identity;
using NanhiDuniya.Service.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NanhiDuniya.Core.Constants;
using Microsoft.Extensions.DependencyInjection;
using NanhiDuniya.Core.Interfaces;
using NanhiDuniya.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

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
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "NanhiDuniyaUserManagementAPI",
        ValidAudience = "NanhiDuniyaUserManagementAPIClient",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisismycustomSecretkeyforauthentication"))
    };
});

// Adding Jwt Bearer
//builder.Services.AddAuthentication().AddJwtBearer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        b => b.AllowAnyHeader()
            .AllowAnyOrigin()
            .AllowAnyMethod());
});
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

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

