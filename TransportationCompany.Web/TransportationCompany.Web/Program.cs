using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using TransportationCompany.ApplicationServices;
using TransportationCompany.ApplicationServices.JourneyServices;
using TransportationCompany.ApplicationServices.PassengersServices;
using TransportationCompany.ApplicationServices.TicketsServices;
using TransportationCompany.Core.Entities;
using TransportationCompany.DataAccess;
using TransportationCompany.DataAccess.Repository;
using TransportationCompany.DataAccess.RepositoryEntities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(); 
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(typeof(MapperProfile));

string connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<TransportationCompanyContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

//builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddTransient<ITicketAppService, TicketAppService>();
builder.Services.AddTransient<IJourneyAppService, JourneyAppService>();
builder.Services.AddTransient<IPassengersAppService, PassengersAppService>();

builder.Services.AddTransient<IRepository<int, Ticket>, TicketRepository>();
builder.Services.AddTransient<IRepository<int, Journey>, JourneyRepository>();
builder.Services.AddTransient<IRepository<int, Passengers>, PassengersRepository>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
