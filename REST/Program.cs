using CosmosOdyssey.REST.Data;
using CosmosOdyssey.REST.Models;
using CosmosOdyssey.REST.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
//     .AddScoped<ICompanyRepository, CompanyRepo>()
//     .AddScoped<ICustomerRepository, CustomerRepo>()
//     .AddScoped<ILegRepository, LegRepo>()
//     .AddScoped<IPlanetRepository, PlanetRepo>()
//     .AddScoped<IPriceListRepository, PriceListRepo>()
//     .AddScoped<IProviderRepository, ProviderRepo>()
//     .AddScoped<IReservationRepository, ReservationRepo>()
//     .AddScoped<IRouteInfoRepository, RouteInfoRepo>()

builder.Services.AddHttpClient();
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddHostedService<DataSeeder>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run("http://*:8080");

