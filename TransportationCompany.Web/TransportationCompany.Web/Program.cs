using AutoMapper.Execution;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Org.BouncyCastle.Crypto.Tls;
using Serilog;
using System.Configuration;
using System.Text;
using System.Xml.Linq;
using TransportationCompany.ApplicationServices;
using TransportationCompany.ApplicationServices.JourneyServices;
using TransportationCompany.ApplicationServices.PassengersServices;
using TransportationCompany.ApplicationServices.TicketsServices;
using TransportationCompany.Core.Entities;
using TransportationCompany.DataAccess;
using TransportationCompany.DataAccess.Repository;
using TransportationCompany.DataAccess.RepositoryEntities;
using TransportationCompany.Shared.Config;
using TransportationCompany.Web;
using TransportationCompany.Web.Auth;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
Log.Information("Starting up!");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Logging.AddSerilog();
builder.Services.AddAutoMapper(typeof(MapperProfile));

string connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<TransportationCompanyContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddTransient<ITicketAppService, TicketAppService>();
builder.Services.AddTransient<IJourneyAppService, JourneyAppService>();
builder.Services.AddTransient<IPassengersAppService, PassengersAppService>();
builder.Services.AddTransient<IChecker, Checker>();

builder.Services.AddTransient<IJwtIssuerOptions, JwtIssuerFactory>();

builder.Services.AddTransient<IRepository<int, Ticket>, TicketRepository>();
builder.Services.AddTransient<IRepository<int, Journey>, JourneyRepository>();
builder.Services.AddTransient<IRepository<int, Passenger>, PassengersRepository>();

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.AddDbContext<TransportationCompanyContext>(options =>
{
    options.UseSqlServer(connectionString);
}, ServiceLifetime.Scoped);

builder.Services.AddSwaggerGen(op =>
{
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
    });
    op.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description = "JWT Authorization hader usin the Bearer scheme. \r\n\r\nEnter yout token in the text input below.\r\n",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    op.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new String[]{ }
        }
    });
});

var tokenValidationSettings = builder.Services.BuildServiceProvider().GetService<IOptions<JwtTokenValidationSettings>>().Value;

builder.Services.AddAuthentication(op =>
{
    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(op =>
{
    op.RequireHttpsMetadata = false;
    op.SaveToken = true;
    op.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = tokenValidationSettings.ValidIssuer,
        ValidAudience = tokenValidationSettings.ValidAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenValidationSettings.SecretKey)),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.Configure<JwtTokenValidationSettings>(builder.Configuration.GetSection("JwtTokenValidationSettings"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 7;
    options.Password.RequiredUniqueChars = 4;
}).AddEntityFrameworkStores<TransportationCompanyContext>().AddDefaultTokenProviders();

//var tokenValidationSettings = builder.Configuration.GetSection("JwtTokenValidationSettings").Get<JwtTokenValidationSettings>();
/*
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = tokenValidationSettings.ValidIssuer,
            ValidateAudience = true,
            ValidAudience = tokenValidationSettings.ValidAudience,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenValidationSettings.SecretKey)),
            ValidateIssuerSigningKey = true,
        };
    });
*/
builder.Services.AddHttpClient();
/*
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuerSigningKey = true,
        };
    });

*/
var app = builder.Build();
app.InitDb();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transportation Company API v1"));

}
app.MapControllerRoute(name: "default", pattern: "{controller=swagger}/{actSystem.AggregateException: 'Some services are not able to be constructed (Error while validating ion=Index}/{id?}");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
