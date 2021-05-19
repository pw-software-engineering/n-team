using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace ServerApiMockup.MockupApiControllers
{
    [Route("api-client")]
    [ApiController]
    public class SearchApiController : ControllerBase
    {
        public SearchApiController() { }

        [HttpGet("hotels")]
        public IActionResult GetHotels(string hotelName = null, string country = null, string city = null, int pageNumber = 1, int pageSize = 10)
        {
            byte[] imgRaw = System.IO.File.ReadAllBytes($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Resources/picture.png");
            string imgBase64 = "data:image/png;base64," + Convert.ToBase64String(imgRaw);
            HotelPreviewInfo templatePreviewInfo = new HotelPreviewInfo()
            {
                HotelID = 1,
                HotelName = "Best hotel",
                Country = "Poland",
                City = "Warsaw",
                PreviewPicture = imgBase64
            };
            List<HotelPreviewInfo> hotels = new List<HotelPreviewInfo>();
            for(int i = 0; i < 4; i++)
            {
                hotels.Add(templatePreviewInfo.Clone());
                hotels[i].HotelID = i + 1;
            }
            hotels[0].City = "Cracow";
            hotels[1].HotelName = "Mediocre hotel";
            hotels[2].HotelName = "Bad hotel";
            hotels[3].Country = "France";
            hotels[3].City = "Paris";
            hotels = hotels
                .Where(h => hotelName == null ? true : h.HotelName.ToLower().Contains(hotelName.ToLower()))
                .Where(h => country == null ? true : h.Country.ToLower().Contains(country.ToLower()))
                .Where(h => city == null ? true : h.City.ToLower().Contains(city.ToLower()))
                .ToList();
            return new JsonResult(
                hotels,
                new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    IgnoreNullValues = true
                });
        }

        [HttpGet("hotels/{hotelID}")]
        public IActionResult GetHotelDetails(int hotelID)
        {
            //Thread.Sleep(3000);
            byte[] imgRawRoom = System.IO.File.ReadAllBytes($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Resources/picture.png");
            string imgBase64Room = "data:image/png;base64," + Convert.ToBase64String(imgRawRoom);
            byte[] imgRawStock = System.IO.File.ReadAllBytes($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Resources/stock-photo.jpg");
            string imgBase64Stock = "data:image/jpg;base64," + Convert.ToBase64String(imgRawStock);
            HotelDetails hotelDetails = new HotelDetails()
            {
                HotelName = "Best Hotel",
                HotelDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                Country = "Poland",
                City = "Warsaw",
                HotelPictures = new List<string>()
                {
                    imgBase64Room,
                    imgBase64Stock
                }
            };
            return new JsonResult(
                hotelDetails,
                new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    IgnoreNullValues = true
                });
        }

        [HttpGet("hotels/{hotelID}/offers")]
        public IActionResult GetHotelOffers([FromRoute] int hotelID, [FromQuery] OfferFilter offerFilter, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            Console.WriteLine("GET HOTEL OFFERS");
            if(pageNumber > 2)
            {
                return new JsonResult(new List<OfferPreviewInfo>());
            }
            List<OfferPreviewInfo> offers = new List<OfferPreviewInfo>();
            byte[] imgRawRoom = System.IO.File.ReadAllBytes($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Resources/picture.png");
            string imgBase64Room = "data:image/png;base64," + Convert.ToBase64String(imgRawRoom);
            OfferPreviewInfo previewInfo = new OfferPreviewInfo()
            {
                OfferID = 1,
                CostPerAdult = 10.4,
                CostPerChild = 8.5,
                MaxGuests = 10,
                OfferTitle = "Best Offer",
                OfferPreviewPicture = imgBase64Room
            };
            for(int i = 0; i < 3; i++)
            {
                OfferPreviewInfo offerPreview = previewInfo.Clone();
                offerPreview.OfferID = i + 1;
                offerPreview.CostPerAdult = previewInfo.CostPerAdult * (i + 1);
                offerPreview.CostPerChild = previewInfo.CostPerChild * (i + 1);
                offerPreview.OfferTitle += (i + 1).ToString();
                offers.Add(offerPreview);
            }
            return new JsonResult(
                offers,
                new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    IgnoreNullValues = true
                });
        }

        [HttpGet("hotels/{hotelID}/offers/{offerID}")]
        public IActionResult GetHotelOfferDetails([FromRoute] int hotelID, [FromRoute] int offerID)
        {
            if(hotelID == 10 && offerID == 10)
            {
                return NotFound();
            }
            byte[] imgRawRoom = System.IO.File.ReadAllBytes($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Resources/picture.png");
            string imgBase64Room = "data:image/png;base64," + Convert.ToBase64String(imgRawRoom);
            byte[] imgRawStock = System.IO.File.ReadAllBytes($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Resources/stock-photo.jpg");
            string imgBase64Stock = "data:image/jpg;base64," + Convert.ToBase64String(imgRawStock);
            bool isDeleted = false, isActive = true;
            if(offerID == 2)
            {
                isActive = false;
            }
            else if(offerID == 3)
            {
                isActive = false;
                isDeleted = true;
            }
            OfferView offerView = new OfferView()
            {
                OfferID = 1,
                OfferTitle = "The best offer in the world",
                OfferDescription = "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem.",
                IsDeleted = isDeleted,
                IsActive = isActive,
                CostPerChild = 10.32,
                CostPerAdult = 15.01,
                MaxGuests = 5,
                OfferPictures = new List<string>()
                {
                    imgBase64Room,
                    imgBase64Stock
                }
            };
            return new JsonResult(
                offerView,
                new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    IgnoreNullValues = true
                });
        }
    }

    public class OfferView
    {
        public int OfferID { get; set; }
        public string OfferTitle { get; set; }
        public string OfferDescription { get; set; }
        public List<string> OfferPictures { get; set; }
        public int MaxGuests { get; set; }
        public double CostPerChild { get; set; }
        public double CostPerAdult { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class OfferFilter
    {
        [FromQuery]
        [Required]
        public DateTime? FromTime { get; set; }

        [FromQuery]
        [Required]
        public DateTime? ToTime { get; set; }

        [FromQuery]
        public int? MinGuests { get; set; }

        [FromQuery]
        public int? CostMin { get; set; }

        [FromQuery]
        public int? CostMax { get; set; }
    }

    class OfferPreviewInfo
    {
        public int OfferID { get; set; }
        public string OfferTitle { get; set; }
        public int MaxGuests { get; set; }
        public double CostPerChild { get; set; }
        public double CostPerAdult { get; set; }
        public string OfferPreviewPicture { get; set; }

        public OfferPreviewInfo Clone()
        {
            return new OfferPreviewInfo()
            {
                OfferID = OfferID,
                OfferTitle = OfferTitle,
                MaxGuests = MaxGuests,
                CostPerChild = CostPerChild,
                CostPerAdult = CostPerAdult,
                OfferPreviewPicture = OfferPreviewPicture
            };
        }
    }


    class HotelPreviewInfo
    {
        public int HotelID { get; set; }
        public string HotelName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PreviewPicture { get; set; }

        public HotelPreviewInfo Clone()
        {
            return new HotelPreviewInfo()
            {
                HotelID = HotelID,
                HotelName = HotelName,
                Country = Country,
                City = City,
                PreviewPicture = PreviewPicture
            };
        }
    }

    class HotelDetails
    {
        public string HotelName { get; set; }
        public string HotelDescription { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public List<string> HotelPictures { get; set; }

        public HotelDetails Clone()
        {
            return new HotelDetails()
            {
                HotelName = HotelName,
                HotelDescription = HotelDescription,
                Country = Country,
                City = City,
                HotelPictures = new List<string>(HotelPictures)
            };
        }
    }
}
