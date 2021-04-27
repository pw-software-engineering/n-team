using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess
{
    public interface IClientTokenDataAccess
    {
        bool CheckIfClientExists(int clientID);
    }
}
