﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Client_Module.Models;
using Client_Module.ViewsTagID.Layout;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Client_Module.Controllers
{
    public class SearchController : Controller
    {
        public SearchController()
        {
            
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData[LayoutTagID.NavSelectedBtnKey] = LayoutTagID.NavSearchBtnID;
            base.OnActionExecuting(context);
        }

        [Route("/hotels")]
        public IActionResult Hotels()
        {
            return View();
        }

        [Route("/hotels/{hotelID}")]
        public IActionResult DetailedHotel(int hotelID)
        {
            return View();
        }

        [Route("/hotels/{hotelID}/offers")]
        public IActionResult HotelOffers(int hotelID, HotelOffersModel hotelOffersModel)
        {
            return View();
        }

        [Route("/hotels/{hotelID}/offers/{offerID}")]
        public IActionResult DetailedHotelOffer(int hotelID, int offerID)
        {
            return View();
        }
    }
}