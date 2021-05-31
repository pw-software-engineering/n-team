using Hotel.Models;
using System.Collections.Generic;

namespace Hotel.ViewModels
{
    public class RoomsIndexViewModel
    {
        public IEnumerable<Room> Rooms { get; set; }
        public Paging Paging { get; set; }
        public string HotelRoomNumber { get; set; }

        public RoomsIndexViewModel()
        {
            Rooms = new List<Room>();
            Paging = new Paging();
            HotelRoomNumber = null;
        }

        public RoomsIndexViewModel(IEnumerable<Room> rooms, Paging paging, string hotelRoomNumber)
        {
            Rooms = rooms;
            Paging = paging;
            HotelRoomNumber = hotelRoomNumber;
        }
    }
}
