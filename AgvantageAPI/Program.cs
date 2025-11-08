using AgvantageAPI.Models;
using Microsoft.EntityFrameworkCore;
using AgvantageAPI.Services;
using Microsoft.OpenApi.Models;
using AgvantageAPI.DTO;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ✅ Configure a Swagger document named "v1"
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Agvantage API",
        Version = "v1"
    });
});

builder.Services.AddDbContext<dbContext>(
     options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ILog, AgvantageAPI.Services.Log>();

var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // This must match the doc name "v1" above
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agvantage API v1");
    });


app.UseAuthorization();

app.MapControllers();

app.Run();
