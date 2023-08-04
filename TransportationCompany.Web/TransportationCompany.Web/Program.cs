using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Xml.Linq;
using TransportationCompany.ApplicationServices;
using TransportationCompany.ApplicationServices.JourneyServices;
using TransportationCompany.ApplicationServices.PassengersServices;
using TransportationCompany.ApplicationServices.TicketsServices;
using TransportationCompany.Core.Entities;
using TransportationCompany.DataAccess;
using TransportationCompany.DataAccess.Repository;
using TransportationCompany.DataAccess.RepositoryEntities;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
Log.Information("Starting up!");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(op =>
    op.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Transportation Company",
        Description = "An ASP.NET Core Web API for managing a transportation company",
        Contact = new OpenApiContact
        {
            Name = "Jair Alfonso Acosta",
            //Url = new Uri("")
        }
    })
);
builder.Logging.AddSerilog();
builder.Services.AddAutoMapper(typeof(MapperProfile));

string connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<TransportationCompanyContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddTransient<ITicketAppService, TicketAppService>();
builder.Services.AddTransient<IJourneyAppService, JourneyAppService>();
builder.Services.AddTransient<IPassengersAppService, PassengersAppService>();

builder.Services.AddTransient<IRepository<int, Ticket>, TicketRepository>();
builder.Services.AddTransient<IRepository<int, Journey>, JourneyRepository>();
builder.Services.AddTransient<IRepository<int, Passengers>, PassengersRepository>();

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

//app.MapControllerRoute(name: "default", pattern: "{controller=swagger}/{action=Index}/{id?}");


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
