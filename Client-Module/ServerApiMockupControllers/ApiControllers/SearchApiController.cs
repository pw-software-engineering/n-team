﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace ServerApiMockup.MockupApiControllers
{
    [Route("api-client/hotels")]
    [ApiController]
    public class SearchApiController : ControllerBase
    {
        public SearchApiController() { }

        [HttpGet("")]
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

        [HttpGet("{hotelID}")]
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
