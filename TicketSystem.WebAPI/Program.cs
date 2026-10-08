using Microsoft.EntityFrameworkCore;
using TicketSystem.WebAPI.Context;
using TicketSystem.WebAPI.Mappings;


var builder = WebApplication.CreateBuilder(args);

MapsterConfig.RegisterMappings();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServer")));




var app = builder.Build();




app.MapGet("/", () => "Hello World!");



app.Run();
