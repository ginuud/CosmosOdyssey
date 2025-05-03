using CosmosOdyssey.REST.Data;
using CosmosOdyssey.REST.Models;
using CosmosOdyssey.REST.Services;
using Microsoft.EntityFrameworkCore;
using REST.Data.Repos;
using REST.Interfaces;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services
    .AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddScoped<IReservationRepository, ReservationRepo>();
//     .AddScoped<ICompanyRepository, CompanyRepo>()
//     .AddScoped<ICustomerRepository, CustomerRepo>()
//     .AddScoped<ILegRepository, LegRepo>()
//     .AddScoped<IPlanetRepository, PlanetRepo>()
//     .AddScoped<IPriceListRepository, PriceListRepo>()
//     .AddScoped<IProviderRepository, ProviderRepo>()
//     .AddScoped<IRouteInfoRepository, RouteInfoRepo>()

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
    {
        builder
        .SetIsOriginAllowed(_ => true)
        .AllowCredentials()
        .AllowAnyMethod()
        .AllowAnyHeader();
    }));

builder.Services.AddHttpClient<IDataService, DataService>();
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddHostedService<DataSeeder>();
builder.Services.AddHostedService<TravelPriceSyncService>();

var app = builder.Build();

using (var scope = ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
using (var context = scope.ServiceProvider.GetService<DataContext>())
{
    context?.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyPolicy");

app.UseHttpsRedirection();
app.MapControllers();

app.Run("http://*:80");

