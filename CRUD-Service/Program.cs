using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DATABASE_URL");
builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

var forecast =  Enumerable.Range(1, 5).Select(index =>
    new WeatherForecast
    (
        Guid.NewGuid(),
        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        Random.Shared.Next(-20, 55),
        summaries[Random.Shared.Next(summaries.Length)]
    ))
    .ToArray();

app.MapGet("/random", () =>
{
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

DatabaseContext databaseContext = app.Services.GetRequiredService<DatabaseContext>();

if (await databaseContext.Database.EnsureCreatedAsync())
{
    await databaseContext.WeatherForecasts.AddRangeAsync(forecast);
    await databaseContext.SaveChangesAsync();
}

app.MapGet("/read", async () =>
{
    return await databaseContext.WeatherForecasts.ToListAsync();
})
.WithName("GetWeatherForecastFromDatabase")
.WithOpenApi();

app.MapPost("/create", async (WeatherForecast weatherForecast) =>
{
    await databaseContext.WeatherForecasts.AddAsync(weatherForecast);
    await databaseContext.SaveChangesAsync();
    return weatherForecast;
})
.WithName("PostWeatherForecastToDatabase")
.WithOpenApi();

app.MapPut("/update", async (WeatherForecast weatherForecast) =>
{
    databaseContext.WeatherForecasts.Update(weatherForecast);
    await databaseContext.SaveChangesAsync();
    return weatherForecast;
})
.WithName("PutWeatherForecastToDatabase")
.WithOpenApi();

app.MapDelete("/delete/{id}", async (Guid id) =>
{
    WeatherForecast weatherForecast = await databaseContext.WeatherForecasts.FindAsync(id);
    databaseContext.WeatherForecasts.Remove(weatherForecast);
    await databaseContext.SaveChangesAsync();
})
.WithName("DeleteWeatherForecastFromDatabase")
.WithOpenApi();

app.Run();
