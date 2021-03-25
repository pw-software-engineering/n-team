﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.Models
{
    public class HotelRoomDb
    {
        //Properties
        public int RoomID { get; set; }
        public int HotelID { get; set; }
        public string HotelRoomNumber { get; set; }
        //Navigational Properties
        public HotelInfoDb Hotel { get; set; }
        public List<OfferHotelRoomDb> OfferHotelRooms { get; set; }
    }
}
