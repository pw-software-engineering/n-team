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
        
        public bool UpdateClientInfo(int clientID, string username, string email)
        {
            bool result = false;
            using (var transaction = _dbContext.Database.BeginTransaction())
			{
                var client = _dbContext.Clients.Find(clientID);
                if (result = client != null)
				{
                    client.Username = username ?? client.Username;
                    client.Email = email ?? client.Email;
                    _dbContext.SaveChanges();
                    transaction.Commit();
                }
			}
            return result;
        }
    }
}
