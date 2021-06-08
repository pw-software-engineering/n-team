using Server.RequestModels.Client;
using Server.Services.Result;

namespace Server.Services.Client
{
    public interface IClientAccountService
    {
        #region /client
        public IServiceResult UpdateClientInfo(int clientID, ClientInfoUpdate editClientInfo);
        public IServiceResult GetClientInfo(int clientID);
        #endregion

        #region /client/login
        public IServiceResult Login(ClientCredentials clientCredentials);
        #endregion
    }
}
