namespace Hotel.Models
{
    public class ReservationObject
    {
        public Reservation Reservation { get; set; }
        public Client Client { get; set; }
        public RoomInfo Room { get; set; }
    }
}
