using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TransportationCompany.Client.Data;
using TransportationCompany.Client.Utils.Config;
using TransportationCompany.Client.Utils;
using TransportationCompany.Shared.Config;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("TransportationCompanyClientContextConnection") ?? throw new InvalidOperationException("Connection string 'TransportationCompanyClientContextConnection' not found.");

//builder.Services.AddDbContext<TransportationCompanyClientContext>(options =>
//  options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<TransportationCompanyClientUser>(options => options.SignIn.RequireConfirmedAccount = true)
//  .AddEntityFrameworkStores<TransportationCompanyClientContext>();

// Add services to the container.
builder.Services.AddAuthorization(op =>
{
    op.AddPolicy("UserOnly", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
    });
});
var authorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

builder.Services.Configure<CookiePolicyOptions>(op =>
{
    op.CheckConsentNeeded = context => true;
    op.MinimumSameSitePolicy = SameSiteMode.None;
});
builder.Services.AddRazorPages();
builder.Services.Configure<JwtTokenValidationSettings>(builder.Configuration.GetSection(nameof(JwtTokenValidationSettings)));
builder.Services.AddSingleton<IJwtTokenValidationSettings, JwtTokenValidationSettingsFactory>();

builder.Services.Configure<JwtTokenIssuerSettings>(builder.Configuration.GetSection(nameof(JwtTokenIssuerSettings)));
builder.Services.AddSingleton<IJwtTokenIssuerSettings, JwtTokenIssuerSettingsFactory>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IClaimPrincipalManager, ClaimPrincipalManager>();

builder.Services.Configure<AuthenticationSettings>(builder.Configuration.GetSection(nameof(AuthenticationSettings)));
builder.Services.AddSingleton<IAuthenticationSettings, AuthenticationSettingsFactory>();

var serviceProvider = builder.Services.BuildServiceProvider();

var authenticationSettings = serviceProvider.GetService<IAuthenticationSettings>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddCookie(JwtBearerDefaults.AuthenticationScheme, op =>
    {
        op.LoginPath = authenticationSettings.LoginPath;
        op.AccessDeniedPath = authenticationSettings.AccessDeniedPath;
        op.Events = new CookieAuthenticationEvents
        {
            OnValidatePrincipal = RefreshTokenMonitor.ValidateAsync
        };
    });


builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IActionContextAccessor, ActionContextAccessor>();

builder.Services.AddHttpClient();

builder.Services.AddHttpClient("WebApi", (client) =>
{
    client.BaseAddress = new Uri(builder.Configuration["JwtTokenIssuerSettings:BaseAddress"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

app.Run();
