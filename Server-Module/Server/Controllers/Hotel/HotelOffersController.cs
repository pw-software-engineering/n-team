using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services.OfferService;
using Server.Services.Response;
using Server.ViewModels;
using System;
using System.Linq;

namespace Server.Controllers.Hotel
{
    [ApiController]
    [Route("/api-hotel/")]
    [Authorize(AuthenticationSchemes = "HotelTokenScheme")]
    public class HotelOffersController : Controller
    {
        private readonly IOfferService service;

        public HotelOffersController(IOfferService service)
        {
            this.service = service;
        }

        [HttpGet("offers")]
        public IActionResult GetOffers(bool? isActive, int pageNumber = 1, int pageSize = 10)
        {
            int hotelId = GetHotelID();

            IServiceResult result = service.GetHotelOffers(new Paging(pageSize, pageNumber), hotelId, isActive);
            JsonResult jsonResult = new JsonResult(result.ResponseBody)
            {
                StatusCode = (int)result.StatusCode
            };
            return jsonResult;
        }

        [HttpGet("offers/{offerID}")]
        public IActionResult GetOffer(int offerID)
        {
            int hotelId = GetHotelID();

            IServiceResult result = service.GetOffer(offerID, hotelId);
            JsonResult jsonResult = new JsonResult(result.ResponseBody)
            {
                StatusCode = (int)result.StatusCode
            };
            return jsonResult;
        }

        [HttpPost("offers")]
        public IActionResult AddOffer([FromBody] OfferView offer)
        {
            int hotelId = GetHotelID();

            IServiceResult result = service.AddOffer(offer, hotelId);

            JsonResult jsonResult = new JsonResult(result.ResponseBody)
            {
                StatusCode = (int)result.StatusCode
            };
            return jsonResult;
        }

        [HttpPatch("offers/{offerID}")]
        public IActionResult EditOffer(int offerID, OfferUpdateInfo updateInfo)
        {
            int hotelId = GetHotelID();

            IServiceResult result = service.UpdateOffer(offerID, hotelId, updateInfo);

            JsonResult jsonResult = new JsonResult(result.ResponseBody)
            {
                StatusCode = (int)result.StatusCode
            };
            return jsonResult;
        }

        [HttpDelete("offers/{offerID}")]
        public IActionResult DeleteOffer(int offerID)
        {
            int hotelId = GetHotelID();

            IServiceResult result = service.DeleteOffer(offerID, hotelId);

            JsonResult jsonResult = new JsonResult(result.ResponseBody)
            {
                StatusCode = (int)result.StatusCode
            };
            return jsonResult;
        }

        private int GetHotelID()
        {
            var ids = from claim in HttpContext.User.Claims
                      where claim.Type == "hotelId"
                      select claim.Value;
            return Convert.ToInt32(ids.Single());
        }
    }
}
