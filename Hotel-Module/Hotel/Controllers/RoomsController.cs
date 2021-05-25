﻿using Hotel.Models;
using Hotel.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;

namespace Hotel.Controllers
{
    public class RoomsController : Controller
    {
        private readonly HttpClient _httpClient;

        public RoomsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(nameof(DefaultHttpClient));
        }


        [HttpGet("/rooms")]
        public async Task<IActionResult> Index([FromQuery] string roomNumber, [FromQuery] Paging paging)
        {
            NameValueCollection query = HttpUtility.ParseQueryString("");
            if (!string.IsNullOrWhiteSpace(roomNumber))
                query["roomNumber"] = roomNumber.ToString();
            query["pageNumber"] = paging.PageNumber.ToString();
            query["pageSize"] = paging.PageSize.ToString();

            try
            {
                //IEnumerable<Room> rooms = await _httpClient.GetFromJsonAsync<IEnumerable<Room>>("rooms?" + query.ToString());
                IEnumerable<Room> rooms = new List<Room>
                {
                    new Room
                    {
                        RoomID = 1,
                        HotelRoomNumber = "1",
                        OfferID = new List<int>{102, 103}
                    },
                    new Room
                    {
                        RoomID = 2,
                        HotelRoomNumber = "2",
                        OfferID = new List<int>{1, 2, 3, 4}
                    }
                };
                RoomsIndexViewModel roomsVM = new RoomsIndexViewModel(rooms, paging, roomNumber);
                return View(roomsVM);
            }
            catch (HttpRequestException e)
            {
                return StatusCode((int)(e.StatusCode ?? HttpStatusCode.InternalServerError));
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<PartialViewResult> OfferRowPartial(int offerID)
        {
            try
            {
                Offer offer = await _httpClient.GetFromJsonAsync<Offer>("offers/" + offerID.ToString());
                offer.OfferID = offerID;
                ViewBag.ErrorCode = null;
                return PartialView("_OfferRow", offer);
            }
            catch (HttpRequestException e)
            {
                ViewBag.ErrorCode = (int)(e.StatusCode ?? HttpStatusCode.InternalServerError);
                return PartialView("_OfferRow", null);
            }
            catch (Exception)
            {
                ViewBag.ErrorCode = (int)HttpStatusCode.InternalServerError;
                return PartialView("_OfferRow", null);
            }
        }
    }
}
