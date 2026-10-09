using Carter;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Threading.RateLimiting;
using TicketSystem.WebAPI.Context;
using TicketSystem.WebAPI.Mappings;


var builder = WebApplication.CreateBuilder(args);

// Configuration
var env = builder.Environment.EnvironmentName; //Development, Production, Staging, Test
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
    .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: false);

// Database Configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    string con = builder.Configuration.GetConnectionString("SqlServer")!;
    options.UseSqlServer(con);
});

// Carter
builder.Services.AddCarter();

// Response Compression
builder.Services.AddResponseCompression();

// OpenAPI
builder.Services.AddOpenApi();

// CORS
builder.Services.AddCors();

// Mapster
MapsterConfig.RegisterMappings();

// CORS
builder.Services.AddCors();

// Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", o =>
    {
        o.PermitLimit = 100;
        o.Window = TimeSpan.FromSeconds(1);
        o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        o.QueueLimit = 100;
    });
});





var app = builder.Build();

// OpenAPI & Scalar
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();
}

// HTTPS Redirection
app.UseHttpsRedirection();

// Static Files
app.UseStaticFiles();

// CORS
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
    .SetPreflightMaxAge(TimeSpan.FromSeconds(10))
);

// Response Compression
app.UseResponseCompression();

// Rate Limiting
app.UseRateLimiter();

// Carter
app.MapCarter();


app.Run();
