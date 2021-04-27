using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_Module.Authentication
{
    public class ClientInfo
    {
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string Surname { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string Username { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string Email { get; set; }

        public override string ToString()
        {
            return $"{Name} | {Surname} | {Username} | {Email}";
        }
    }
}
