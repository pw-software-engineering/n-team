using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.Models
{
    public class Client
    {
        //Properties
        public int ClientID { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        //Navigational Properties
        public IEnumerable<ClientReservation> ClientReservations { get; set; }
        public IEnumerable<ClientReview> ClientReviews { get; set; }
    }
}
