using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServerApiMockup.MockupApiControllers
{
    [Route("api/client")]
    [ApiController]
    public class ClientReservationsController : ControllerBase
    {
        public ClientReservationsController() { }

        [HttpGet("reservations")]
        public IActionResult ClientReservations(int pageNumber = 1, int pageSize = 10)
        {
            ReservationData reservationData = new ReservationData();
            reservationData.HotelInfoPreview = new HotelInfoPreview()
            {
                HotelID = 1,
                HotelName = "Best hotel ever",
                Country = "Poland",
                City = "Warsaw"
            };
            reservationData.ReservationInfo = new ReservationInfo()
            {
                ReservationID = 11,
                From = DateTime.Now.AddDays(-10),
                To = DateTime.Now,
                NumberOfAdults = 2,
                NumberOfChildren = 2
            };
            //Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //Console.WriteLine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            byte[] imgRaw = System.IO.File.ReadAllBytes($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Resources/picture.png");
            reservationData.OfferInfoPreview = new OfferInfoPreview()
            {
                OfferID = 2,
                OfferTitle = "Best offer",
                //data:image/png;base64,iVBORw0KGgoA
                OfferPreviewPicture = Convert.ToBase64String(imgRaw)
            };
            List<ReservationData> reservations = new List<ReservationData>();
            reservations.Add(reservationData);
            reservations.Add(reservationData);
            reservations.Add(reservationData);
            return Ok(JsonSerializer.Serialize(
                reservations, 
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
        }
    }

    public class ReservationData
    {
        public HotelInfoPreview HotelInfoPreview { get; set; }
        public ReservationInfo ReservationInfo { get; set; }
        public OfferInfoPreview OfferInfoPreview { get; set; }
    }

    public class HotelInfoPreview
    {
        public int HotelID { get; set; }
        public string HotelName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }

    public class ReservationInfo
    {
        public int ReservationID { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int NumberOfChildren { get; set; }
        public int NumberOfAdults { get; set; }
        public int? ReviewID { get; set; }
    }

    public class OfferInfoPreview
    {
        public int OfferID { get; set; }
        public string OfferTitle { get; set; }
        public string OfferPreviewPicture { get; set; }
    }
}
