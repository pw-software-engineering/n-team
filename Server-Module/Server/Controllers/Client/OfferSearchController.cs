using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Server.RequestModels;
using Server.Services.OfferSearchService;
using Server.Services.Result;

namespace Server.Controllers.Client
{
    //[Authorize(AuthenticationSchemes = ClientTokenDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("/api-client")]
    public class OfferSearchController : Controller
    {
        private readonly IOfferSearchService _offerSearchService;
        public OfferSearchController(IOfferSearchService offerSearchService)
        {
            _offerSearchService = offerSearchService;
        }

        [HotelOfferSearchValidateModel]
        [HttpGet("hotels/{hotelID:int}/offers")]
        public IActionResult GetHotelOffers([FromRoute] int hotelID, [FromQuery] Paging paging, [FromQuery] OfferFilter offerFilter)
        {
            //Console.WriteLine($"Is model valid: {ModelState.IsValid}");
            Console.WriteLine($"hotelID: {hotelID}\nPaging: {paging.pageNumber} | {paging.pageSize}\nOffer Filter: {offerFilter.From} -> {offerFilter.To} : {offerFilter.MinCost} | {offerFilter.MaxCost} | {offerFilter.MinGuests}");
            //return Ok();
            return _offerSearchService.GetHotelOffers(hotelID, paging, offerFilter);
        }

        [HttpGet("hotels/{hotelID:int}/offers/{offerID:int}")]
        public IActionResult GetHotelOfferDetails(int hotelID, int offerID)
        {
            return _offerSearchService.GetHotelOfferDetails(hotelID, offerID);
        }
    }

    public class HotelOfferSearchValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }
            string errorsString = string.Empty;
            foreach(var entry in context.ModelState)
            {
                if(entry.Value.Errors.Count > 0)
                {
                    foreach(var error in entry.Value.Errors)
                    {
                        errorsString += error.ErrorMessage + '\n';
                    }
                }
            }
            context.Result = new ServiceResult(
                HttpStatusCode.BadRequest, 
                new Error(errorsString));
        }
    }
}
