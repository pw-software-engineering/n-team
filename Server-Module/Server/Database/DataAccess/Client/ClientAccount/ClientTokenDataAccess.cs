using Server.Authentication.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess.Client
{
    public class ClientTokenDataAccess : IClientTokenDataAccess
    {
        private readonly ServerDbContext _dbContext;
        public ClientTokenDataAccess(ServerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CheckIfClientExists(int clientID)
        {
            return _dbContext.Clients.Any(client => client.ClientID == clientID);
        }
    }
}
