using Microsoft.EntityFrameworkCore;
using WebApi.Interfaces;
using WebApi.Models;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<postgresContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("local")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Dependency injection for our services
builder.Services.AddScoped<ISettingsService, SettingsService>();
builder.Services.AddScoped<ISleepDataService, SleepDataService>();
builder.Services.AddScoped<IUserDataService, UserDataService>();
builder.Services.AddScoped<IDeviceService, DeviceService>();

//If URLs are not defined in env then use default.
var aspNetCoreUrls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
if (string.IsNullOrEmpty(aspNetCoreUrls))
{
    builder.WebHost.UseUrls(new string[] {
		"http://*:80",
		//"https://*:443",
    });
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<postgresContext>();

    // Apply database migrations
    dbContext.Database.Migrate();

    // Seed the database
    SeedData.Initialize(dbContext);
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
