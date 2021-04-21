using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Database.DataAccess;
using Server.Services.OfferService;
using Server.ViewModels;

namespace Server.Controllers.Hotel
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "HotellTokenScheme")]
    public class HotelOffersController : Controller
    {
        private readonly IDataAccess dataAccess;
        private readonly IMapper mapper;

        public HotelOffersController(IDataAccess dataAccess, IMapper mapper)
        {
            this.dataAccess = dataAccess;
            this.mapper = mapper;
        }

        [HttpPost("/offers")]
        //[Route("/offers")]
        public IActionResult AddOffer(OfferView view)
        {
            OfferService service = new OfferService(dataAccess, mapper);

            return StatusCode(200);
        }
    }
}
