
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
            return !(_dbContext.Clients.Find(clientID) is null);
        }
    }
}
