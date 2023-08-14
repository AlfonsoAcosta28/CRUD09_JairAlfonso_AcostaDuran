using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportationCompany.ApplicationServices;
using TransportationCompany.ApplicationServices.JourneyServices;
using TransportationCompany.ApplicationServices.PassengersServices;
using TransportationCompany.ApplicationServices.TicketsServices;
using TransportationCompany.Core.Entities;
using TransportationCompany.DataAccess;
using TransportationCompany.DataAccess.Repository;
using TransportationCompany.DataAccess.RepositoryEntities;

namespace TransportationCompany.UnitTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MapperProfile));


            services.AddDbContext<TransportationCompanyContext>(options => options.UseInMemoryDatabase("DataTest"));

            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddTransient<ITicketAppService, TicketAppService>();
            services.AddTransient<IJourneyAppService, JourneyAppService>();
            services.AddTransient<IPassengersAppService, PassengersAppService>();

            services.AddTransient<IRepository<int, Ticket>, TicketRepository>();
            services.AddTransient<IRepository<int, Journey>, JourneyRepository>();
            services.AddTransient<IRepository<int, Passenger>, PassengersRepository>();
        }

        public IConfiguration Configuration { get; }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
