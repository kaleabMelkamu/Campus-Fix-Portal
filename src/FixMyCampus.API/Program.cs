using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using FixMyCampus.Infrastructure.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MyCampusDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();