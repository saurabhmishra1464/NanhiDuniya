
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using NanhiDuniya.MessageBus;
using NanhiDuniya.MessageBus.MassTransit;
using NanhiDuniya.MessageBus.SQL;
using NanhiDuniya.Services.EmailApi.Configurations;
using NanhiDuniya.Services.EmailApi.Entities;
using NanhiDuniya.Services.EmailApi.Extentions;
using NanhiDuniya.Services.EmailApi.Middleware;
using NanhiDuniya.Services.EmailApi.Resources;
using NanhiDuniya.Services.EmailApi.Services.Implementations;
using NanhiDuniya.Services.EmailApi.Services.Interfaces;
using Serilog;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddDbContext<NanhiDuniyaDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddScoped(typeof(IRepository<>), typeof(SqlRepository<>));

// Register MassTransit with RabbitMQ
builder.Services.AddMassTransitWithRabbitMq();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NanhiDuniya User Management API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme
        {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
        },
        new string[] { }
    }
    });
});
builder.Services.Configure<ApiBehaviorOptions>(o =>
{
    o.InvalidModelStateResponseFactory = actionContext =>
    {
        var res = new ApiResponse<object>(false, "One or more validation errors occurred.", null, StatusCodes.Status400BadRequest, actionContext.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToArray());
        return new ObjectResult(res);
    };
});
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));
//builder.Services.AddMassTransitWithRabbitMq(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder
            .WithOrigins("http://localhost:7777")  // Allow requests from frontend
            .AllowAnyHeader()                     // Allow headers like Authorization
            .AllowAnyMethod()                     // Allow GET, POST, PUT, etc.
            .AllowCredentials());                 // Allow cookies (if needed)
});



var app = builder.Build();
app.UseCors("AllowFrontend");
app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();
//app.UseHttpsRedirection();// commented if for k8s
app.UseAuthorization();

app.MapControllers();
app.Run();
