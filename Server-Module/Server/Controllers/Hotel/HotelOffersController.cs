using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Database.DataAccess;
using Server.Services.OfferService;
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

        [HttpPost("/offers")]
        public IActionResult AddOffer([FromBody] OfferView offer)
        {
            var ids = from claim in HttpContext.User.Claims
                        where claim.Type == "hotelId"
                        select claim.Value;
            int hotelId = Convert.ToInt32(ids.Single());

            int offerID = service.AddOffer(offer, hotelId);

            return new JsonResult(new { offerID });
        }
    }
}
