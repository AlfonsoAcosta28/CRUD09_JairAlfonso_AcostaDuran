using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportationCompany.Accounts.Dto;
using TransportationCompany.ApplicationServices.TicketsServices;
using TransportationCompany.Core.Entities;
using TransportationCompany.DataAccess;

namespace TransportationCompany.UnitTest
{
    [TestFixture]
    public class TicketTest
    {
        protected TestServer server;
        private ITicketAppService repository;

        [OneTimeSetUp]
        [SetUp]
        public async void Setup()
        {
            this.server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            var manager = server.Host.Services.GetService<TransportationCompanyContext>();
            repository = server.Host.Services.GetService<ITicketAppService>();

            var zitacuaro = new City { Id = 1, Name = "Zitacuro" };
            await manager.Cities.AddAsync(zitacuaro);
            var morelia = new City { Id = 2, Name = "Morelia" };
            await manager.Cities.AddAsync(morelia);

            var passanger1 = await manager.Passengers.AddAsync(new Passengers { Id = 1, FirstName = "Jair", LastName = "Acosta", Age = 19 });
            var passanger2 = await manager.Passengers.AddAsync(new Passengers { Id = 2, FirstName = "Diana", LastName = "Acosta", Age = 20 });
            var passanger3 = await manager.Passengers.AddAsync(new Passengers { Id = 3, FirstName = "Gina", LastName = "Acosta", Age = 24 });

            var jorney1 = await manager.Journeys.AddAsync(new Journey { Id = 1, Arrival = DateTime.Now, Departure = DateTime.Now, DestinationId = zitacuaro, OriginId = morelia });
            var jorney2 = await manager.Journeys.AddAsync(new Journey { Id = 2, Arrival = DateTime.Now, Departure = DateTime.Now, DestinationId = morelia , OriginId = zitacuaro} );

        }
        [Order(0)]
        [Test]
        public async Task GetAllTickets_Test()
        {
            var insertFirts = await repository.AddTicketAsync(new TicketDto() 
                {PassengerId = new Passengers() { Id = 1}, JourneyId = new Journey() { Id = 1} });

            var insertSecond = await repository.AddTicketAsync(new TicketDto()
            { PassengerId = new Passengers() { Id = 2 }, JourneyId = new Journey() { Id = 2 } });

            var list = await repository.GetTicketsAsync();

            Assert.IsNull(list);
            Assert.AreEqual(list.Count, 2);
        }
        }
}
