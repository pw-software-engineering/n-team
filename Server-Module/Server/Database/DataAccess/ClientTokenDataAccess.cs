using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess
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
            return _dbContext.Clients.FirstOrDefault(client => client.ClientID == clientID) != null;
        }
    }
}
