using System.Collections.Generic;

namespace Server.Database.Models
{
    public class ClientDb
    {
        //Properties
        public int ClientID { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        //Navigational Properties
        public List<ClientReservationDb> ClientReservations { get; set; }
        public List<ClientReviewDb> ClientReviews { get; set; }
    }
}
