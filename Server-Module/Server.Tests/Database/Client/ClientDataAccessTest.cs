using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using Server.AutoMapper;
using Server.Database;
using Server.Database.DataAccess;
using Server.Database.DataAccess.Client;
using Server.Database.Models;
using Server.RequestModels;
using Server.RequestModels.Client;
using Server.ViewModels;
using Server.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Server.Tests.Database.Client
{
    public class ClientDataAccessTest : IDisposable
    {
        #region TestsSetup
        public ClientDataAccessTest()
        {
            var serviceProvider = new ServiceCollection()
                                    .AddEntityFrameworkSqlServer()
                                    .BuildServiceProvider();
            var configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();
            var builder = new DbContextOptionsBuilder<ServerDbContext>();
            builder.UseSqlServer(configurationBuilder.GetConnectionString("ClientDAClientTest"))
                    .UseInternalServiceProvider(serviceProvider);

            _context = new ServerDbContext(builder.Options, false);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            Seed();

            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new ClientAutoMapperProfile());
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

        #region GetClientInfo
        [Fact]
        public void GetClientInfo_NonExistentClientID_ReturnsNull()
        {
            int clientID = -1;

            ClientInfoView clientInfoView = _dataAccess.GetClientInfo(clientID);

            Assert.Null(clientInfoView);
        }

        [Fact]
        public void GetClientInfo_ValidClientID_ReturnsClientInfoView()
        {
            int clientID = 1;

            ClientInfoView clientInfoView = _dataAccess.GetClientInfo(clientID);
            ClientDb clientDb = _context.Clients.Find(clientID);

            Assert.Equal(clientDb.Name, clientInfoView.Name);
            Assert.Equal(clientDb.Surname, clientInfoView.Surname);
            Assert.Equal(clientDb.Email, clientInfoView.Email);
            Assert.Equal(clientDb.Username, clientInfoView.Username);
        }
        #endregion

        #region UpdateClientInfo
        [Fact]
        public void UpdateClientInfo_UsernameAndEmailNull_DoesNothing()
        {
            int clientID = 1;
            ClientInfoUpdate clientInfoUpdate = new ClientInfoUpdate()
            {
                Username = null,
                Email = null
            };

            _dataAccess.UpdateClientInfo(clientID, clientInfoUpdate);
            ClientDb client = _context.Clients.Find(clientID);

            Assert.NotNull(client);
            Assert.NotNull(client.Username);
            Assert.NotNull(client.Email);
        }
        [Fact]
        public void UpdateClientInfo_UsernameNullEmailGiven_UpdatesEmail()
        {
            int clientID = 2;
            ClientInfoUpdate clientInfoUpdate = new ClientInfoUpdate()
            {
                Username = null,
                Email = "jelonek@melonek.eu"
            };

            _dataAccess.UpdateClientInfo(clientID, clientInfoUpdate);
            ClientDb client = _context.Clients.Find(clientID);

            Assert.NotNull(client);
            Assert.NotNull(client.Username);
            Assert.Equal(clientInfoUpdate.Email, client.Email);
        }
        [Fact]
        public void UpdateClientInfo_UsernameGivenEmailNull_UpdatesUsername()
        {
            int clientID = 1;
            ClientInfoUpdate clientInfoUpdate = new ClientInfoUpdate()
            {
                Username = "jelonek",
                Email = null
            };

            _dataAccess.UpdateClientInfo(clientID, clientInfoUpdate);
            ClientDb client = _context.Clients.Find(clientID);

            Assert.NotNull(client);
            Assert.NotNull(client.Email);
            Assert.Equal(clientInfoUpdate.Username, client.Username);
        }
        [Fact]
        public void UpdateClientInfo_UsernameAndEmailGiven_UpdatesUsernameAndEmail()
        {
            int clientID = 3;
            ClientInfoUpdate clientInfoUpdate = new ClientInfoUpdate()
            {
                Username = "jelonek",
                Email = "jelonek@melonek.eu"
            };

            _dataAccess.UpdateClientInfo(clientID, clientInfoUpdate);
            ClientDb client = _context.Clients.Find(clientID);

            Assert.NotNull(client);
            Assert.Equal(clientInfoUpdate.Username, client.Username);
            Assert.Equal(clientInfoUpdate.Email, client.Email);
        }
        #endregion

        #region GetRegisteredClientID
        [Fact]
        public void GetRegisteredClientID_LoginOrPasswordNullOrEmpty_Null()
        {
            int? clientIDNull = _dataAccess.GetRegisteredClientID(null, null);
            int? clientIDEmpty = _dataAccess.GetRegisteredClientID(string.Empty, string.Empty);
            int? clientIDPartial = _dataAccess.GetRegisteredClientID("TestEmail1@testdomain.com", string.Empty);

            Assert.False(clientIDNull.HasValue);
            Assert.False(clientIDEmpty.HasValue);
            Assert.False(clientIDPartial.HasValue);
        }

        [Fact]
        public void GetRegisteredClientID_InvalidUsernameOrPassword_Null()
        {
            int? clientIDPassword = _dataAccess.GetRegisteredClientID("NonexistentLoginEmail", "TestPassword1");
            int? clientIDUsername = _dataAccess.GetRegisteredClientID("TestEmail1@testdomain.com", "InvalidPassword123#@!");

            Assert.False(clientIDPassword.HasValue);
            Assert.False(clientIDUsername.HasValue);
        }

        [Fact]
        public void GetRegisteredClientID_ValidUsernameAndPassword_DatabaseClientID()
        {
            int expectedClientID = 1;
            string login = "TestEmail1@testdomain.com";
            string password = "TestPassword1";

            int? clientID = _dataAccess.GetRegisteredClientID(login, password);

            Assert.Equal(expectedClientID, clientID.Value);
        }
        #endregion
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
