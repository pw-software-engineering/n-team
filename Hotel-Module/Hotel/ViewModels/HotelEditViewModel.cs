using Hotel.Models;

namespace Hotel.ViewModels
{
    public class HotelEditViewModel
    {
        public HotelInfo HotelInfo { get; set; }
        public bool ChangePreviewPicture { get; set; }
        public bool ChangeHotelPictures { get; set; }
    }
}
