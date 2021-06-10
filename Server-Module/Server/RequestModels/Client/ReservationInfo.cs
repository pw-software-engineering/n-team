using System;

namespace Server.RequestModels.Client
{
    public class ReservationInfo
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int NumberOfChildren { get; set; }
        public int NumberOfAdults { get; set; }
    }
}
