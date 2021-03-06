﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Client_Module.Authentication;
using Client_Module.Models;
using Client_Module.ViewsTagID.Layout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Client_Module.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData[LayoutTagID.NavSelectedBtnKey] = LayoutTagID.NavSearchBtnID;
            base.OnActionExecuting(context);
        }

        [HttpGet("/hotels")]
        public IActionResult Hotels()
        {
            return View();
        }

        [HttpGet("/hotels/{hotelID}")]
        public IActionResult DetailedHotel(int hotelID)
        {
            HotelDetailsModel model = new HotelDetailsModel()
            {
                HotelID = hotelID
            };
            return View(model);
        }

        [HttpGet("/hotels/{hotelID}/offers")]
        public IActionResult HotelOffers(int hotelID)
        {
            HotelOfferSearchModel model = new HotelOfferSearchModel()
            {
                HotelID = hotelID
            };
            return View(model);
        }

        [HttpGet("/hotels/{hotelID}/offers/{offerID}")]
        public IActionResult DetailedHotelOffer(int hotelID, int offerID, [FromQuery] DateTime? fromTime, [FromQuery] DateTime? toTime)
        {
            HotelOfferDetailsModel model = new HotelOfferDetailsModel()
            {
                HotelID = hotelID,
                OfferID = offerID,
                FromTime = fromTime,
                ToTime = toTime
            };
            return View(model);
        }
    }
}
