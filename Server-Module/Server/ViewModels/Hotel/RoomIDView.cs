﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.ViewModels.Hotel
{
    public class RoomIDView
    {
        public int RoomID { get; }
        public RoomIDView(int roomID)
        {
            RoomID = roomID;
        }
    }
}