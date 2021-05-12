using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServerApiMockup.MockupApiControllers
{
    [Route("api-client/client/reservations")]
    [ApiController]
    public class ClientApiReservationsController : ControllerBase
    {
        public ClientApiReservationsController() { }

        [HttpGet("")]
        public IActionResult GetClientReservations(int pageNumber = 1, int pageSize = 10)
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
                To = DateTime.Now.AddDays(10),
                NumberOfAdults = 2,
                NumberOfChildren = 2
            };
            //Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //Console.WriteLine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            byte[] imgRaw = System.IO.File.ReadAllBytes($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Resources/picture.png");
            string imgBase64 = "data:image/png;base64," + Convert.ToBase64String(imgRaw);
            reservationData.OfferInfoPreview = new ReservationOfferInfoPreview()
            {
                OfferID = 2,
                OfferTitle = "Best offer",
                //data:image/png;base64,iVBORw0KGgoA
                OfferPreviewPicture = imgBase64
            };
            List<ReservationData> reservations = new List<ReservationData>();
            reservations.Add(reservationData.Clone());
            reservations.Add(reservationData.Clone());
            reservations.Add(reservationData.Clone());
            reservations.Add(reservationData.Clone());
            reservations[0].ReservationInfo.ReservationID = 1;
            reservations[0].ReservationInfo.From = DateTime.Now.AddDays(10);
            reservations[0].ReservationInfo.To = DateTime.Now.AddDays(20);
            reservations[1].ReservationInfo.ReservationID = 2;
            reservations[2].ReservationInfo.ReservationID = 3;
            reservations[2].ReservationInfo.From = new DateTime(1980, 12, 12);
            reservations[2].ReservationInfo.To = new DateTime(1980, 12, 28);
            reservations[3].ReservationInfo.ReservationID = 4;
            reservations[3].ReservationInfo.From = new DateTime(1980, 12, 12);
            reservations[3].ReservationInfo.To = new DateTime(1980, 12, 28);
            reservations[3].ReservationInfo.ReviewID = 3;
            List<ReservationData> pagedReservations = new List<ReservationData>();
            for(int i = (pageNumber - 1) * pageSize; i < pageNumber * pageSize && i < reservations.Count; i++)
            {
                pagedReservations.Add(reservations[i]);
            }
            return new JsonResult(
                pagedReservations,
                new JsonSerializerOptions() {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    IgnoreNullValues = true
                });
        }

        [HttpDelete("{reservationID}")]
        public IActionResult DeleteClientReservation(int reservationID)
        {
            Thread.Sleep(2000);
            return Ok();
        }

        [HttpGet("{reservationID}/review")]
        public IActionResult GetReservationReview(int reservationID)
        {
            JsonSerializerOptions serializerSettings = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true
            };
            if (!(reservationID == 4 || reservationID == 3))
            {
                return NotFound(new { error = "This reservation is not completed" });
            }
            ReviewInfo reviewInfo = new ReviewInfo()
            {
                Content = "This is some sample review content that can be very very very very very very very very very very very very very very very very very very very very very very very very very very very very long",
                ReviewID = 1,
                CreationDate = DateTime.Now,
                Rating = 4,
                ReviewerUsername = "jankowalski99"
            };
            return new JsonResult(reviewInfo, serializerSettings);
        }

        [HttpPut("{reservationID}/review")]
        public IActionResult EditReservationReview(int reservationID)
        {
            if (!(reservationID == 4 || reservationID == 3))
            {
                return NotFound(new { error = "This reservation is not completed" });
            }
            Thread.Sleep(2000);
            return new JsonResult(new { reviewID = 5 });
        }

        [HttpDelete("{reservationID}/review")]
        public IActionResult DeleteReservationReview(int reservationID)
        {
            Thread.Sleep(2000);
            return Ok();
        }
    }

    public class ReservationData
    {
        public HotelInfoPreview HotelInfoPreview { get; set; }
        public ReservationInfo ReservationInfo { get; set; }
        public ReservationOfferInfoPreview OfferInfoPreview { get; set; }

        public ReservationData Clone()
        {
            return new ReservationData()
            {
                HotelInfoPreview = HotelInfoPreview.Clone(),
                ReservationInfo = ReservationInfo.Clone(),
                OfferInfoPreview = OfferInfoPreview.Clone()
            };
        }
    }

    public class HotelInfoPreview
    {
        public int HotelID { get; set; }
        public string HotelName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public HotelInfoPreview Clone()
        {
            return new HotelInfoPreview()
            {
                HotelID = HotelID,
                HotelName = HotelName,
                Country = Country,
                City = City
            };
        }
    }

    public class ReservationInfo
    {
        public int ReservationID { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int NumberOfChildren { get; set; }
        public int NumberOfAdults { get; set; }
        public int? ReviewID { get; set; }

        public ReservationInfo Clone()
        {
            return new ReservationInfo()
            {
                ReservationID = ReservationID,
                From = From,
                To = To,
                NumberOfChildren = NumberOfChildren,
                NumberOfAdults = NumberOfAdults,
                ReviewID = ReviewID
            };
        }
    }

    public class ReservationOfferInfoPreview
    {
        public int OfferID { get; set; }
        public string OfferTitle { get; set; }
        public string OfferPreviewPicture { get; set; }

        public ReservationOfferInfoPreview Clone()
        {
            return new ReservationOfferInfoPreview()
            {
                OfferID = OfferID,
                OfferTitle = OfferTitle,
                OfferPreviewPicture = OfferPreviewPicture
            };
        }
    }

    public class ReviewInfo
    {
        public int ReviewID { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreationDate { get; set; }
        public string ReviewerUsername { get; set; }
    }
}
