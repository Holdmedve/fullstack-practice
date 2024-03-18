

using Microsoft.EntityFrameworkCore;
using Car;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CarDb>(opt => opt.UseInMemoryDatabase("CarDatabase"));
builder.Services.AddSingleton<IDateTime, SystemDateTime>();
builder.Services.AddTransient<ILogger>(p =>
{
    var loggerFactory = p.GetRequiredService<ILoggerFactory>();
    return loggerFactory.CreateLogger("my logger");
});
// builder.Services.Configure<DbConfig>(builder.Configuration.GetSection("DbConfig"));

// builder.Services.Configure<DbConfig>(builder.Configuration.GetSection("DbConfig"));
builder.Services.AddSingleton<DbConfig>(
    builder.Configuration.GetSection("DbConfig").Get<DbConfig>()
);

builder.Logging.AddConsole();

var app = builder.Build();

app.MapPost("/cars/", CarHandler.CreateCar);
app.MapGet("/cars/{id}", CarHandler.GetCar);
app.MapDelete("/cars/{id}", CarHandler.DeleteCar);
app.MapPut("/cars/", CarHandler.UpdateCar);

app.Run();
