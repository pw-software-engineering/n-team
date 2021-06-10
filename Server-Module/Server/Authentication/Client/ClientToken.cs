using Newtonsoft.Json;
using System;

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
