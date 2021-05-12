using AutoMapper;
using Server.Database.Models;
using Server.ViewModels;
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

        public int? GetRegisteredClientID(string login, string password)
        {
            return _dbContext.Clients.FirstOrDefault(client => client.Email == login && client.Password == password)?.ClientID;
        }

        public ClientInfoView GetClientInfo(int clientID)
        {
            return _mapper.Map<ClientInfoView>(_dbContext.Clients.Find(clientID));
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
