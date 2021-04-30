using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.AutoMapper;
using Server.Database;
using Server.Database.DataAccess;
using Server.Database.Models;
using Server.Models;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Server.Tests.Database
{
    public class ClientDataAccessTest : IDisposable
    {
        #region TestsSetup
        public ClientDataAccessTest()
        {
            var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkSqlServer()
            .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ServerDbContext>();
            builder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=ServerDbTestsClients;Trusted_Connection=True;MultipleActiveResultSets=true")
                    .UseInternalServiceProvider(serviceProvider);

            _context = new ServerDbContext(builder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            if (!_context.HotelRooms.Any())
                Seed();

            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new AutoMapperProfile());
            });
            _mapper = config.CreateMapper();

            _dataAccess = new ClientDataAccess(_mapper, _context);
        }
        private void Seed()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Clients ON");
                _context.Clients.AddRange(
                    new ClientDb { ClientID = 1, Username = "TestUsername1", Name = "TestName1", Surname = "TestSurname1", Password = "TestPassword1", Email = "TestEmail1@testdomain.com", },
                    new ClientDb { ClientID = 2, Username = "TestUsername2", Name = "TestName2", Surname = "TestSurname2", Password = "TestPassword2", Email = "TestEmail2@testdomain.com", },
                    new ClientDb { ClientID = 3, Username = "TestUsername3", Name = "TestName3", Surname = "TestSurname3", Password = "TestPassword3", Email = "TestEmail3@testdomain.com" });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Clients OFF;");

                _context.SaveChanges();
                transaction.Commit();
            }
        }
        #endregion

        private ServerDbContext _context;
        private IMapper _mapper;
        private ClientDataAccess _dataAccess;

		#region UpdateClientInfo
		[Fact]
        public void UpdateClientInfo_UsernameAndEmailNull_DoesNothing()
        {
            int clientID = 1;
            string username = null, email = null;

            _dataAccess.UpdateClientInfo(clientID, username, email);
            ClientDb client = _context.Clients.Find(clientID);

            Assert.NotNull(client);
            Assert.NotNull(client.Username);
            Assert.NotNull(client.Email);
        }
        [Fact]
        public void UpdateClientInfo_UsernameNullEmailGiven_UpdatesEmail()
        {
            int clientID = 2;
            string username = null, email = "jelonek@melonek.eu";

            _dataAccess.UpdateClientInfo(clientID, username, email);
            ClientDb client = _context.Clients.Find(clientID);

            Assert.NotNull(client);
            Assert.NotNull(client.Username);
            Assert.Equal(client.Email, email);
        }
        [Fact]
        public void UpdateClientInfo_UsernameGivenEmailNull_UpdatesUsername()
        {
            int clientID = 1;
            string username = "jelonek", email = null;

            _dataAccess.UpdateClientInfo(clientID, username, email);
            ClientDb client = _context.Clients.Find(clientID);

            Assert.NotNull(client);
            Assert.Equal(client.Username, username);
            Assert.NotNull(client.Email);
        }
        [Fact]
        public void UpdateClientInfo_UsernameAndEmailGiven_UpdatesUsernameAndEmail()
        {
            int clientID = 3;
            string username = "jelonek", email = "jelonek@melonek.eu";

            _dataAccess.UpdateClientInfo(clientID, username, email);
            ClientDb client = _context.Clients.Find(clientID);

            Assert.NotNull(client);
            Assert.Equal(client.Username, username);
            Assert.Equal(client.Email, email);
        }
        #endregion

        #region GetRegisteredClientID
        [Fact]
        public void GetRegisteredClientID_UsernameOrPasswordNullOrEmpty_Null()
        {
            int? clientIDNull = _dataAccess.GetRegisteredClientID(null, null);
            int? clientIDEmpty = _dataAccess.GetRegisteredClientID(string.Empty, string.Empty);
            int? clientIDPartial = _dataAccess.GetRegisteredClientID("TestUsername1", string.Empty);

            Assert.False(clientIDNull.HasValue);
            Assert.False(clientIDEmpty.HasValue);
            Assert.False(clientIDPartial.HasValue);
        }

        [Fact]
        public void GetRegisteredClientID_InvalidUsernameOrPassword_Null()
        {
            int? clientIDPassword = _dataAccess.GetRegisteredClientID("NonexistentUsername", "TestPassword1");
            int? clientIDUsername = _dataAccess.GetRegisteredClientID("TestUsername1", "InvalidPassword123#@!");

            Assert.False(clientIDPassword.HasValue);
            Assert.False(clientIDUsername.HasValue);
        }

        [Fact]
        public void GetRegisteredClientID_ValidUsernameAndPassword_DatabaseClientID()
        {
            int expectedClientID = 1;
            string username = "TestUsername1";
            string password = "TestPassword1";

            int? clientID = _dataAccess.GetRegisteredClientID(username, password);

            Assert.Equal(expectedClientID, clientID.Value);
        }
        #endregion
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
