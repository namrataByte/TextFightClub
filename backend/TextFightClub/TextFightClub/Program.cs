using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // ✅ Force camelCase for all JSON responses
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddHttpClient();

// ✅ Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000") // your React app
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TextFightClub API",
        Version = "v1",
        Description = "API for AI text battle between two bots"
    });
});

var app = builder.Build();

// ✅ Enable CORS
app.UseCors();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TextFightClub API v1");
    c.RoutePrefix = string.Empty; // loads Swagger at root
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
