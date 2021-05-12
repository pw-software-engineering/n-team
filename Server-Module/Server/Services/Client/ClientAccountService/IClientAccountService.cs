using Server.Database.Models;
using Server.RequestModels;
using Server.Services.Result;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
