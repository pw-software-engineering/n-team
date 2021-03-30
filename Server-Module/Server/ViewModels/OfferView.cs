using Server.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.ViewModels
{
    public class OfferView
    {
        public bool isActive { get; set; }
        public string offerTitle { get; set; }
        public double costPerChild { get; set; }
        public double costPerAdult { get; set; }
        public uint maxGuests { get; set; }
        public string description { get; set; }
        public string offerPreviewPicture { get; set; }
        public List<string> pictures { get; set; }
        public List<string> rooms { get; set; }
        public OfferView(OfferDb offer)
        {
            isActive = offer.IsActive;
            offerTitle = offer.OfferTitle;
            costPerChild = offer.CostPerChild;
            costPerAdult = offer.CostPerAdult;
            maxGuests = offer.MaxGuests;
            description = offer.Description;
            offerPreviewPicture = offer.OfferPreviewPicture;
            pictures = new List<string>();
            foreach (OfferPictureDb picture in offer.OfferPictures)
                pictures.Add(picture.Picture);
            rooms = new List<string>();
            foreach (OfferHotelRoomDb room in offer.OfferHotelRooms)
                rooms.Add(room.Room.HotelRoomNumber);
        }
    }
}
