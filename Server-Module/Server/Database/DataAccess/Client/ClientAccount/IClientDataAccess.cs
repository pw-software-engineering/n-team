using Server.RequestModels;
using Server.RequestModels.Client;
using Server.ViewModels;
using Server.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess.Client
{
	public interface IClientDataAccess
	{
        #region /client GET
        public ClientInfoView GetClientInfo(int clientID);
        #endregion

        #region /client PATCH
        public void UpdateClientInfo(int clientID, ClientInfoUpdate editClientInfo);
		#endregion

		#region /client/login
		public int? GetRegisteredClientID(string login, string password);
        #endregion
    }
}
