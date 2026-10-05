using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using FixMyCampus.API.ExceptionHandlers;
using FixMyCampus.Application.Common.Interfaces;
using FixMyCampus.Application.Services;
using FixMyCampus.Infrastructure.Authentication;
using FixMyCampus.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Database
builder.Services.AddDbContext<MyCampusDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<MyCampusDbContext>());

// 2. JWT & Authentication Configuration
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));
var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>() ?? new JwtSettings();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
        };
    });

builder.Services.AddAuthorization();

// 3. Application Services
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITicketService, TicketService>();

// 4. CORS for Angular Frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// 5. Controllers with JSON Enum string serializer
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// 6. OpenAPI Versioning (v1 Current / v2 Preview)
builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "FixMyCampus API - v1 (Current)";
        document.Info.Version = "v1";
        document.Info.Description = "FixMyCampus Issue Reporting & Maintenance Tracker - Version 1 (Production MVP).";
        return Task.CompletedTask;
    });
});

builder.Services.AddOpenApi("v2", options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "FixMyCampus API - v2 (Feature)";
        document.Info.Version = "v2";
        document.Info.Description = "FixMyCampus - Version 2.";
        return Task.CompletedTask;
    });
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Auto-migrate and seed demo accounts and initial tickets
try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<MyCampusDbContext>();
    await DbInitializer.SeedAsync(db);
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogWarning(ex, "Database seeding/migration skipped or encountered an error.");
}

// 7. Scalar API Reference with v1/v2 Versioning
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Scalar for v1: accessible at /scalar/v1
    app.MapScalarApiReference("/scalar/v1", options =>
    {
        options.WithTitle("FixMyCampus API - v1 (Current)")
               .WithOpenApiRoutePattern("/openapi/v1.json");
    });

    // Scalar for v2: accessible at /scalar/v2
    app.MapScalarApiReference("/scalar/v2", options =>
    {
        options.WithTitle("FixMyCampus API - v2 (Preview)")
               .WithOpenApiRoutePattern("/openapi/v2.json");
    });

    // Default redirect /scalar -> /scalar/v1
    app.MapGet("/scalar", () => Results.Redirect("/scalar/v1"));
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();