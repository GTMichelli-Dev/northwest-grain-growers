using DevExpress.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Seed25.Models;

//using DevExpress.AspNetCore.Reporting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IReportPrinter, ReportPrinter>();
//builder.Services.AddDevExpressControls();

// Add services to the container.
builder.Services
    .AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Add Swagger/OpenAPI services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Seed25 API", Version = "v1" });
});

// Configure DbContext with connection string from appsettings.json
builder.Services.AddDbContext<Seed_DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Seed_DataContext"))
);

var app = builder.Build();

// Enable Swagger middleware in development (or always, if you prefer)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Seed25 API V1");
    });
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
//app.UseDevExpressControls();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
