using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services.OfferService;
using Server.Services.Response;
using Server.ViewModels;
using System;
using System.Linq;

namespace Server.Controllers.Hotel
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "HotelTokenScheme")]
    public class HotelOffersController : Controller
    {
        private readonly IOfferService service;

        public HotelOffersController(IOfferService service)
        {
            this.service = service;
        }

        [HttpGet("api-hotel/offers")]
        public IActionResult GetOffers(bool? isActive, int pageNumber = 1, int pageSize = 10)
        {
            var ids = from claim in HttpContext.User.Claims
                      where claim.Type == "hotelId"
                      select claim.Value;
            int hotelId = Convert.ToInt32(ids.Single());

            IServiceResult result = service.GetHotelOffers(new Paging(pageSize, pageNumber), hotelId, isActive);
            JsonResult jsonResult = new JsonResult(result.ResponseBody)
            {
                StatusCode = (int)result.StatusCode
            };
            return jsonResult;
        }

        [HttpGet("api-hotel/offers/{offerID}")]
        public IActionResult GetOffer(int offerID)
        {
            var ids = from claim in HttpContext.User.Claims
                      where claim.Type == "hotelId"
                      select claim.Value;
            int hotelId = Convert.ToInt32(ids.Single());

            IServiceResult result = service.GetOffer(offerID, hotelId);
            JsonResult jsonResult = new JsonResult(result.ResponseBody)
            {
                StatusCode = (int)result.StatusCode
            };
            return jsonResult;
        }

        [HttpPost("api-hotel/offers")]
        public IActionResult AddOffer([FromBody] OfferView offer)
        {
            var ids = from claim in HttpContext.User.Claims
                      where claim.Type == "hotelId"
                      select claim.Value;
            int hotelId = Convert.ToInt32(ids.Single());

            IServiceResult result = service.AddOffer(offer, hotelId);

            JsonResult jsonResult = new JsonResult(result.ResponseBody)
            {
                StatusCode = (int)result.StatusCode
            };
            return jsonResult;
        }
    }
}
