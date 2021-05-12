using AutoMapper;
using Server.Database.Models;
using Server.RequestModels;
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

        public int? GetRegisteredClientID(string username, string password)
        {
            return _dbContext.Clients.FirstOrDefault(client => client.Username == username && client.Password == password)?.ClientID;
        }

        public ClientInfoView GetClientInfo(int clientID)
        {
            return _mapper.Map<ClientInfoView>(_dbContext.Clients.Find(clientID));
        }

        public void UpdateClientInfo(int clientID, EditClientInfo editClientInfo)
        {
            var client = _dbContext.Clients.Find(clientID);
            client.Username = string.IsNullOrWhiteSpace(editClientInfo.Username) ? client.Username : editClientInfo.Username;
            client.Email = string.IsNullOrWhiteSpace(editClientInfo.Email) ? client.Email : editClientInfo.Email;
            _dbContext.SaveChanges();
        }
    }
}
