
namespace Server.Database.DataAccess.Client
{
    public interface IClientTokenDataAccess
    {
        bool CheckIfClientExists(int clientID);
    }
}
