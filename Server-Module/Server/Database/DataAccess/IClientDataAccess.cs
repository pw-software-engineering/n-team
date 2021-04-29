using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess
{
	public interface IClientDataAccess
	{
		#region /client PATCH
		public void UpdateClientInfo(int clientID, string username, string email);
		#endregion
	}
}
