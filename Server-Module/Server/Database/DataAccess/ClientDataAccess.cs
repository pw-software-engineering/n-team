using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess
{
    public class ClientDataAccess : IClientDataAccess
    {
        private readonly IMapper _mapper;
        private readonly ServerDbContext _dbContext;
        public ClientDataAccess(IMapper mapper, ServerDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public int? GetRegisteredClientID(string username, string password)
        {
            return _dbContext.Clients.FirstOrDefault(client => client.Username == username && client.Password == password)?.ClientID;
        }

        public void UpdateClientInfo(int clientID, string username, string email)
        {
            var client = _dbContext.Clients.Find(clientID);
            client.Username = string.IsNullOrWhiteSpace(username) ? client.Username : username;
            client.Email = string.IsNullOrWhiteSpace(email) ? client.Email : email;
            _dbContext.SaveChanges();
        }
    }
}
