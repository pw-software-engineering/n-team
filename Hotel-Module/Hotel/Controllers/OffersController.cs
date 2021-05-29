﻿using Hotel.Models;
using Hotel.ViewModels;
using Hotel_Module.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
    [Authorize(AuthenticationSchemes = HotelTokenCookieDefaults.AuthenticationScheme)]
    public class OffersController : Controller
    {
        private readonly HttpClient _httpClient;

        public OffersController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(nameof(DefaultHttpClient));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _httpClient.DefaultRequestHeaders.Add(
                ServerApiConfig.TokenHeaderName,
                HttpContext.User.Claims.First(c => c.Type == HotelCookieTokenManagerOptions.AuthStringClaimType).Value);
        }

        [HttpGet("/offers")]
        public async Task<IActionResult> Index([FromQuery] bool? isActive, [FromQuery] Paging paging)
        {
            NameValueCollection query = HttpUtility.ParseQueryString("");
            if (isActive.HasValue)
                query["isActive"] = isActive.ToString();
            query["pageNumber"] = paging.PageNumber.ToString();
            query["pageSize"] = paging.PageSize.ToString();

            try
            {
                IEnumerable<OfferPreview> response = await _httpClient.GetFromJsonAsync<IEnumerable<OfferPreview>>("offers?" + query.ToString());
                OffersIndexViewModel offersVM = new OffersIndexViewModel(response, paging, isActive);
                return View(offersVM);
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

        [HttpGet("/offers/{offerID}")]
        public async Task<IActionResult> Details([FromRoute] int offerID)
        {
            try
            {
                Offer offer = await _httpClient.GetFromJsonAsync<Offer>("offers/" + offerID.ToString());
                return View(offer);
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

        [HttpGet("/offers/{offerID}/edit")]
        public async Task<IActionResult> Edit([FromRoute] int offerID)
        {
            try
            {
                Offer offer = await _httpClient.GetFromJsonAsync<Offer>("offers/" + offerID.ToString());
                return View(new OfferEditViewModel(offer));
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

        [HttpPost("/offers/{offerID}/edit")]
        public async Task<IActionResult> Edit([FromForm] OfferEditViewModel offerViewModel)
        {
            OfferUpdateInfo offer = new OfferUpdateInfo
            {
                IsActive = offerViewModel.Offer.IsActive,
                OfferTitle = offerViewModel.Offer.OfferTitle,
                Description = offerViewModel.Offer.Description,
                OfferPreviewPicture = offerViewModel.ChangePreviewPicture ? (offerViewModel.Offer.OfferPreviewPicture ?? "") : null,
                OfferPictures = offerViewModel.ChangeOfferPictures ? (offerViewModel.Offer.Pictures ?? new List<string>()) : null
            };
            HttpContent content = JsonContent.Create(offer);

            return await CheckForConnectionError(_httpClient.PatchAsync("offers/" + offerViewModel.Offer.OfferID.ToString(), content));
        }

        [HttpPost("/offers/{offerID}/changeActive")]
        public async Task<IActionResult> ChangeActive([FromRoute] int offerID, [FromQuery] bool isActive)
        {
            OfferUpdateInfo offer = new OfferUpdateInfo
            {
                IsActive = isActive
            };
            HttpContent content = JsonContent.Create(offer);

            await CheckForConnectionError(_httpClient.PatchAsync("offers/" + offerID.ToString(), content));
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("/offers/add")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("/offers/add")]
        public async Task<IActionResult> Add([FromForm] Offer offer)
        {
            return await CheckForConnectionError(_httpClient.PostAsJsonAsync("offers", offer));
        }

        [HttpGet("/offers/{offerID}/delete")]
        public async Task<IActionResult> Delete([FromRoute] int offerID)
        {
            return await CheckForConnectionError(_httpClient.DeleteAsync("offers/" + offerID.ToString()));
        }

        private async Task<StatusCodeResult> CheckForConnectionError(Task<HttpResponseMessage> responseTask)
        {
            try
            {
                HttpResponseMessage response = await responseTask;
                return StatusCode((int)response.StatusCode);
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
    }
}
