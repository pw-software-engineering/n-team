using Server.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Authentication
{
    public interface ICustomAuthenticationManager
    {
        string Authenticate(string AuthenticationString);

        IDictionary<string, string> Tokens { get; }
    }

    public class HotelAutenticationManager : ICustomAuthenticationManager
    {

        public HotelAutenticationManager(List<HotelInfo> list) 
        {
            hotels = new Dictionary<string, int>();
            foreach(var h in list)
            {
                hotels.Add(h.AccessToken, h.HotelID);
            }
        }
        
        private readonly IDictionary<string, int> hotels;

        private readonly IDictionary<string, string> tokens = new Dictionary<string, string>();

        public IDictionary<string, string> Tokens => tokens;

        public string Authenticate(string AuthenticationString)
        {
            if (!hotels.Any(u => u.Key == AuthenticationString))
            {
                return null;
            }

            int id = hotels.First(x => x.Key == AuthenticationString).Value;

            var token = Guid.NewGuid().ToString();

            tokens.Add(token, id.ToString() );

            return token;
        }
    }
}
