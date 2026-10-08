using Microsoft.EntityFrameworkCore;
using TicketSystem.WebAPI.Context;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServer")));



var app = builder.Build();




app.MapGet("/", () => "Hello World!");



app.Run();
