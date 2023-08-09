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
        private Passengers passangerOne;
        private Passengers passangerTwo;
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

            var passanger1 = await context.Passengers.AddAsync(new Passengers { Id = 1, FirstName = "Jair", LastName = "Acosta", Age = 19 });
            var passanger2 = await context.Passengers.AddAsync(new Passengers { Id = 2, FirstName = "Diana", LastName = "Acosta", Age = 20 });
            
            var jorney1 = await context.Journeys.AddAsync(new Journey { Id = 1, Arrival = DateTime.Now, Departure = DateTime.Now, DestinationId = zitacuaro, OriginId = morelia });
            var jorney2 = await context.Journeys.AddAsync(new Journey { Id = 2, Arrival = DateTime.Now, Departure = DateTime.Now, DestinationId = morelia, OriginId = zitacuaro });

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
                Passenger = passangerOne,
                Journey = jorneyOne,
                Seat = 18,
            });
            var getTicket1 = await repository.GetTicketAsync(addTicket1.Id);

            Assert.IsNotNull(addTicket1);
            Assert.AreEqual(addTicket1.Passenger.Id, getTicket1.Passenger.Id);
            Assert.AreEqual(addTicket1.Journey.Id, getTicket1.Journey.Id);
            Assert.AreEqual(addTicket1.Seat, getTicket1.Seat);

            //Ticket 2
            var addTicket2 = await repository.AddTicketAsync(new TicketDto()
            {
                Id = 2,
                Passenger = passangerTwo,
                Journey = jorneyTwo,
                Seat = 20
            });
            var getTicket2 = await repository.GetTicketAsync(addTicket2.Id);

            Assert.IsNotNull(addTicket2);
            Assert.AreEqual(addTicket2.Passenger.Id, getTicket2.Passenger.Id);
            Assert.AreEqual(addTicket2.Journey.Id, getTicket2.Journey.Id);
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
                Passenger = new Passengers() { Id = 1 }, 
                Journey = new Journey() { Id = 1 }, 
                Seat = 18 
            };
            var result = await repository.GetTicketAsync(ticket.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(ticket.Passenger.Id, result.Passenger.Id);
            Assert.AreEqual(ticket.Journey.Id, result.Journey.Id);
            Assert.AreEqual(ticket.Seat, result.Seat);
        }

        [Order(3)]
        [Test]
        public async Task Edit_Test()
        {
          //  var _ticketAppService = server.Host.Services.GetService<ITicketAppService>();

            var originalTicket = ticketEdit;
          /*  var originalTicket = new TicketDto()
            {
                Journey = jorneyOne,
                Passenger = passangerOne,
                Seat = 1,
                Id = 10
            };
           
            var insertEntity = await repository.AddTicketAsync(originalTicket);
          
            var editTicket = new Ticket() { 
                Journey = jorneyTwo, Passenger = passangerTwo , Seat = 2, Id = originalTicket.Id
            };
          */
            var updateEntity = await repository.EditTicketAsync(ticketEdit);

          //  var checkUpdate = await repository.GetTicketAsync(editTicket.Id);

            Assert.IsNotNull(originalTicket);
            Assert.AreNotEqual(originalTicket.Passenger.Id, updateEntity.Passenger.Id);
            Assert.AreNotEqual(originalTicket.Journey.Id, updateEntity.Journey.Id);
            Assert.AreNotEqual(originalTicket.Seat, updateEntity.Seat);
        }

       /* [Order(4)]
        [Test]
        public async Task Delet_Test(Ticket ticket)
        {
            await repository.DeleteTicketAsync(ticket.Id);
            var checkDelete = await repository.GetTicketAsync(ticket.Id);

            Assert.IsNull(checkDelete);
        }
        */

    }
}
