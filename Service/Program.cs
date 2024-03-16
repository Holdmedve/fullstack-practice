

using Microsoft.EntityFrameworkCore;
using Car;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CarDb>(opt => opt.UseInMemoryDatabase("CarDatabase"));
var app = builder.Build();

app.MapPost("/cars/", CarHandler.CreateCar);
app.MapGet("/cars/{id}", CarHandler.GetCar);

app.Run();
