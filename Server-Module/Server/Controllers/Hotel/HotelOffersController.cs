using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Database.DataAccess;
using Server.Services.OfferService;
using Server.ViewModels;

namespace Server.Controllers.Hotel
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "HotelTokenScheme")]
    public class HotelOffersController : Controller
    {
        private readonly IOfferService service;
        private readonly IHotelTokenDataAccess tokenDataAccess;

        public HotelOffersController(IOfferService service, IHotelTokenDataAccess tokenDataAccess)
        {
            this.service = service;
            this.tokenDataAccess = tokenDataAccess;
        }

        [HttpPost("/offers")]
        public IActionResult AddOffer(OfferView offer)
        {
            return StatusCode(200);
        }
    }
}
