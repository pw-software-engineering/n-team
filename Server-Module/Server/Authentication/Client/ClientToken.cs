using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Authentication.Client
{
    public class ClientToken
    {
        [JsonConstructor]
        private ClientToken(int clientID, DateTime createdAt)
        {
            ID = clientID;
            CreatedAt = createdAt;
        }
        public ClientToken(int ID)
        {
            this.ID = ID;
            CreatedAt = DateTime.Now;
        }

        [JsonProperty(Required = Required.Always)]
        public int ID { get; private set; }

        [JsonProperty(Required = Required.Always)]
        public DateTime CreatedAt { get; private set; }
    }
}
