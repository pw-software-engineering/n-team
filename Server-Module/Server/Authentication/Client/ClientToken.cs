using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Authentication.Client
{
    public class ClientToken
    {
        public ClientToken(int ID)
        {
            this.ID = ID;
            CreatedAt = DateTime.Now;
        }
        public int ID { get; }
        public DateTime CreatedAt { get; }
    }
}
