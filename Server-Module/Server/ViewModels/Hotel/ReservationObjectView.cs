using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.ViewModels.Hotel
{
    public class ReservationObjectView
    {
        public ReservationView Reservation { get; set; }
        public ClientView Client { get; set; }
        public RoomView Room { get; set; }
    }
}
