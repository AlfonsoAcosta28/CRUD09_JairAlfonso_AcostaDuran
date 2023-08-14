using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MySqlX.XDevAPI.Common;
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
        private TransportationCompanyContext context;
        private Passenger passangerOne;
        private Passenger passangerTwo;
        private Journey jorneyOne;
        private Journey jorneyTwo;
        private Ticket ticketEdit;


        [OneTimeSetUp]
        [SetUp]
        public void Setup()
        {
            this.server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            var manager = server.Host.Services.GetService<TransportationCompanyContext>();
            repository = server.Host.Services.GetService<ITicketAppService>();
            context = manager;
            initMethod();

        }

        public async void initMethod()
        {
            var zitacuaro = new City { Id = 1, Name = "Zitacuro" };
            await context.Cities.AddAsync(zitacuaro);
            var morelia = new City { Id = 2, Name = "Morelia" };
            await context.Cities.AddAsync(morelia);

            var passanger1 = await context.Passengers.AddAsync(new Passenger { Id = 1, FirstName = "Jair", LastName = "Acosta", Age = 19 });
            var passanger2 = await context.Passengers.AddAsync(new Passenger { Id = 2, FirstName = "Diana", LastName = "Acosta", Age = 20 });
            var passanger3 = await context.Passengers.AddAsync(new Passenger { Id = 3, FirstName = " Luis", LastName = "Acosta", Age = 20 });
            
            var jorney1 = await context.Journeys.AddAsync(new Journey { Id = 1, Arrival = DateTime.Now, Departure = DateTime.Now, Destination = zitacuaro, Origin = morelia });
            var jorney2 = await context.Journeys.AddAsync(new Journey { Id = 2, Arrival = DateTime.Now, Departure = DateTime.Now, Destination = morelia, Origin = zitacuaro });
            var jorney3 = await context.Journeys.AddAsync(new Journey { Id = 3, Arrival = DateTime.Now, Departure = DateTime.Now, Destination = morelia, Origin = zitacuaro });

            passangerOne = passanger1.Entity;
            passangerTwo = passanger2.Entity;

            jorneyOne = jorney1.Entity;
            jorneyTwo = jorney2.Entity;
        }
        [Order(0)]
        [Test]
        public async Task AddTicket_Test()
        {
           
            var addTicket1 = await repository.AddTicketAsync(new TicketDto()
            {
                Id = 1,
                PassengerId = 1,
                JourneyId = 1,
                Seat = 18,
            });
            var getTicket1 = await repository.GetTicketAsync(addTicket1.Id);

            Assert.IsNotNull(addTicket1);
            Assert.AreEqual(addTicket1.PassengerId, getTicket1.PassengerId);
            Assert.AreEqual(addTicket1.JourneyId, getTicket1.JourneyId);
            Assert.AreEqual(addTicket1.Seat, getTicket1.Seat);

            //Ticket 2
            var addTicket2 = await repository.AddTicketAsync(new TicketDto()
            {
                Id = 2,
                PassengerId = 2,
                JourneyId = 2,
                Seat = 20
            });
            var getTicket2 = await repository.GetTicketAsync(addTicket2.Id);

            Assert.IsNotNull(addTicket2);
            Assert.AreEqual(addTicket2.PassengerId, getTicket2.PassengerId);
            Assert.AreEqual(addTicket2.JourneyId, getTicket2.JourneyId);
            Assert.AreEqual(addTicket2.Seat, getTicket2.Seat);

           
        }
        [Order(1)]
        [Test]
        public async Task GetAllTickets_Test()
        {
           
            var list = await repository.GetTicketsAsync();

            Assert.IsNotNull(list);
            Assert.AreEqual(list.Count, 2);
        }

        [Order(2)]
        [Test]
        public async Task GetTicketById_Test()
        {
            var ticket = new Ticket()
            { 
                Id = 1,
                PassengerId = 1, 
                JourneyId = 1, 
                Seat = 18 
            };
            var result = await repository.GetTicketAsync(ticket.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(ticket.PassengerId, result.PassengerId);
            Assert.AreEqual(ticket.JourneyId, result.JourneyId);
            Assert.AreEqual(ticket.Seat, result.Seat);
        }

        [Order(3)]
        [Test]
        public async Task Edit_Test()
        {
            var originalTicket = await context.Tickets.FindAsync(1); /*await repository.AddTicketAsync(new TicketDto()
            {
                PassengerId = 3,
                JourneyId = 3,
                Seat = 3
            });
            */
            var editedTicket = new TicketDto()
            {
                Id = originalTicket.Id,
                PassengerId = 2,
                JourneyId = 2,
                Seat = 2
            };

            var updateEntity = await repository.EditTicketAsync(editedTicket);

            var checkUpdate = await repository.GetTicketAsync(updateEntity.Id);

            Assert.IsNotNull(updateEntity);
            Assert.IsNotNull(checkUpdate);
            Assert.AreEqual(editedTicket.PassengerId, updateEntity.PassengerId);
            Assert.AreEqual(editedTicket.JourneyId, updateEntity.JourneyId);
            Assert.AreEqual(editedTicket.Seat, updateEntity.Seat);
            Assert.AreEqual(editedTicket.PassengerId, checkUpdate.PassengerId);
            Assert.AreEqual(editedTicket.JourneyId, checkUpdate.JourneyId);
            Assert.AreEqual(editedTicket.Seat, checkUpdate.Seat);
        }
        [Order(4)]
        [Test]
        public async Task Delete_Test()
        {
            var ticketServiceApp = server.Host.Services.GetService<ITicketAppService>();
           
            await ticketServiceApp.DeleteTicketAsync(1); 
            var deletedTicket = await ticketServiceApp.GetTicketAsync(1);
            Assert.IsNull(deletedTicket);
        }

    }
}
