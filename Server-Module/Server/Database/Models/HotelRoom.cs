﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.Models
{
    public class HotelRoom
    {
        //Properties
        public int RoomID { get; set; }
        public int HotelID { get; set; }
        public string HotelRoomNumber { get; set; }
        //Navigational Properties
        public HotelInfo Hotel { get; set; }
        public List<OfferHotelRoom> OfferHotelRooms { get; set; }
    }
}