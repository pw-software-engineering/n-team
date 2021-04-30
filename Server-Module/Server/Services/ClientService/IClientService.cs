using Server.Database.Models;
using Server.Models;
using Server.Services.Response;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.ClientService
{
    public interface IClientService
    {
        #region /client
        public IServiceResult UpdateClientInfo(int clientID, string username, string email);
        #endregion

        #region /client/login
        public IServiceResult Login(string username, string password);
        #endregion
    }
}
