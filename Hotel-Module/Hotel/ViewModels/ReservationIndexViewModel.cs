using Hotel.Models;
using System.Collections.Generic;

namespace Hotel.ViewModels
{
    public class ReservationIndexViewModel
    {
        public IEnumerable<ReservationObject> Reservations { get; set; }
        public Paging Paging { get; set; }
        public bool CurrentOnly { get; set; }
        public string RoomNumber { get; set; }

        public ReservationIndexViewModel()
        {
            Reservations = new List<ReservationObject>();
            Paging = new Paging();
            CurrentOnly = false;
            RoomNumber = null;
        }
    }
}
