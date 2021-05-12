using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.Database;
using Server.Database.DataAccess;
using Server.Database.DataAccess.Client;
using Server.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Server.Tests.Database.Client
{
    public class ClientTokenDataAccessTest : IDisposable
    {

        private ServerDbContext _context;
        private ClientTokenDataAccess _tokenAccess;
        public ClientTokenDataAccessTest()
        {
            var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkSqlServer()
            .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ServerDbContext>();
            builder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=ServerDbTests;Trusted_Connection=True;MultipleActiveResultSets=true")
                    .UseInternalServiceProvider(serviceProvider);

            _context = new ServerDbContext(builder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            if (!_context.Hotels.Any())
                Seed();

            _tokenAccess = new ClientTokenDataAccess(_context);
        }
        private void Seed()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Clients ON");
                _context.Clients.Add(
                    new ClientDb { ClientID = 1, Username = "TestUsername1", Email = "TestEmail1", Name = "TestName1", Surname = "TestSurname1", Password = "TestPassword1" }
                 );
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Clients OFF;");

                transaction.Commit();
            }
        }

        [Fact]
        public void CheckIfClientExists_IDExistsInDatabase_ReturnsTrue()
        {
            int clientID = 1;

            bool doesExist = _tokenAccess.CheckIfClientExists(clientID);

            Assert.True(doesExist);
        }

        [Fact]
        public void CheckIfClientExists_IDNotInDatabase_ReturnsFalse()
        {
            int clientID = 10;

            bool doesExist = _tokenAccess.CheckIfClientExists(clientID);

            Assert.False(doesExist);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
